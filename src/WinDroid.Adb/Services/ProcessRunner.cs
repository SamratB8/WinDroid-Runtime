using System.ComponentModel;
using System.Diagnostics;
using WinDroid.Adb.Models;

namespace WinDroid.Adb.Services;

/// <summary>
/// Runs external processes asynchronously, capturing standard output, standard
/// error, and the exit code, with optional timeout and cancellation support.
/// </summary>
/// <remarks>
/// This is a low-level primitive. It launches the executable directly (never a
/// shell), passes each argument individually through
/// <see cref="ProcessStartInfo.ArgumentList"/>, and does not interpret output or
/// exit codes. ADB-specific behaviour is layered on top of it elsewhere.
/// </remarks>
public sealed class ProcessRunner
{
    /// <summary>
    /// Maximum time to wait for a process to exit after it has been killed, so a
    /// misbehaving process cannot make execution hang indefinitely.
    /// </summary>
    private static readonly TimeSpan TerminationWaitTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Maximum time to wait for redirected output to finish draining after the
    /// process has exited, so a stuck stream cannot make execution hang.
    /// </summary>
    private static readonly TimeSpan OutputDrainTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Runs the given executable asynchronously and captures its output and exit
    /// code.
    /// </summary>
    /// <param name="executablePath">The path to the executable to run.</param>
    /// <param name="arguments">
    /// The arguments to pass. Each entry is added individually to
    /// <see cref="ProcessStartInfo.ArgumentList"/>; no shell quoting is applied.
    /// May be <see langword="null"/> for no arguments. Individual entries must not
    /// be <see langword="null"/>.
    /// </param>
    /// <param name="timeout">
    /// Optional maximum run time. When it elapses the process and its child tree
    /// are terminated and the result reports <see cref="CommandResult.TimedOut"/>.
    /// <see langword="null"/> means no timeout. Must be greater than zero when
    /// supplied.
    /// </param>
    /// <param name="cancellationToken">
    /// When cancelled, the process and its child tree are terminated and a result
    /// with <see cref="CommandResult.Cancelled"/> is returned rather than throwing.
    /// </param>
    /// <returns>A <see cref="CommandResult"/> describing the outcome.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="executablePath"/> is <see langword="null"/>, empty, or
    /// whitespace, or an entry of <paramref name="arguments"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeout"/> is less than or equal to zero.
    /// </exception>
    public async Task<CommandResult> RunAsync(
        string executablePath,
        IReadOnlyList<string>? arguments = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(executablePath);

        if (timeout is { } requestedTimeout && requestedTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                timeout,
                "Timeout must be greater than zero, or null for no timeout.");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        if (arguments is not null)
        {
            foreach (string argument in arguments)
            {
                if (argument is null)
                {
                    throw new ArgumentException(
                        "Argument values must not be null.", nameof(arguments));
                }

                startInfo.ArgumentList.Add(argument);
            }
        }

        // Honour an already-cancelled caller token before spending resources on
        // launching a process only to immediately terminate it.
        if (cancellationToken.IsCancellationRequested)
        {
            return CommandResult.CancellationBeforeStart();
        }

        using var process = new Process { StartInfo = startInfo };

        try
        {
            if (!process.Start())
            {
                return CommandResult.StartupFailure(
                    $"The process '{executablePath}' could not be started.");
            }
        }
        catch (Exception ex) when (
            ex is Win32Exception or InvalidOperationException or PlatformNotSupportedException)
        {
            return CommandResult.StartupFailure(
                $"Failed to start process '{executablePath}'. {ex.Message}");
        }

        // Begin draining both streams immediately. Reading them concurrently
        // prevents a full pipe on one stream from blocking the process (and thus
        // deadlocking) while we wait on the other. No cancellation token is
        // forwarded: each read completes when its stream reaches end-of-file
        // (including once the process is terminated), and its captured text is
        // returned. A read that does not complete within the bounded drain
        // period is reported as an empty string.
        Task<string> stdoutTask = process.StandardOutput.ReadToEndAsync(CancellationToken.None);
        Task<string> stderrTask = process.StandardError.ReadToEndAsync(CancellationToken.None);

        bool timedOut = false;
        bool cancelled = false;

        using (var timeoutCts = new CancellationTokenSource())
        using (var linkedCts =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
        {
            if (timeout is { } activeTimeout)
            {
                timeoutCts.CancelAfter(activeTimeout);
            }

            try
            {
                await process.WaitForExitAsync(linkedCts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                if (!process.HasExited)
                {
                    // Caller cancellation takes precedence over timeout when both
                    // have fired, matching the documented precedence.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        cancelled = true;
                    }
                    else
                    {
                        timedOut = true;
                    }

                    await TerminateProcessAsync(process).ConfigureAwait(false);
                }

                // If the process exited on its own just as the token fired, fall
                // through and report normal completion instead of a spurious
                // timeout or cancellation.
            }
        }

        (string standardOutput, string standardError) =
            await DrainOutputAsync(stdoutTask, stderrTask).ConfigureAwait(false);

        int exitCode = TryGetExitCode(process);

        if (cancelled)
        {
            return CommandResult.Cancellation(exitCode, standardOutput, standardError);
        }

        if (timedOut)
        {
            return CommandResult.Timeout(exitCode, standardOutput, standardError);
        }

        return CommandResult.Completed(exitCode, standardOutput, standardError);
    }

    /// <summary>
    /// Kills the process and its child tree, then waits a bounded amount of time
    /// for it to exit. Tolerates the race where the process has already exited.
    /// </summary>
    private static async Task TerminateProcessAsync(Process process)
    {
        try
        {
            process.Kill(entireProcessTree: true);
        }
        catch (Exception ex) when (
            ex is InvalidOperationException or Win32Exception or NotSupportedException)
        {
            // The process already exited, could not be accessed, or the tree
            // cannot be killed on this platform. Fall through to the bounded wait.
        }

        try
        {
            await process.WaitForExitAsync()
                .WaitAsync(TerminationWaitTimeout)
                .ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // The process did not exit within the bounded wait; stop waiting so
            // execution cannot hang forever.
        }
    }

    /// <summary>
    /// Awaits the two output reads with a bounded wait, returning whatever was
    /// captured. Never throws and never hangs indefinitely.
    /// </summary>
    private static async Task<(string StandardOutput, string StandardError)> DrainOutputAsync(
        Task<string> stdoutTask,
        Task<string> stderrTask)
    {
        Task bothReads = Task.WhenAll(stdoutTask, stderrTask);

        try
        {
            // The reads complete when each stream reaches end-of-file, which
            // happens once the process exits. The bounded wait guards against a
            // process that never releases a stream.
            await bothReads.WaitAsync(OutputDrainTimeout).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // Draining exceeded the bounded wait; return whatever completed.
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException)
        {
            // A redirected stream broke (for example, after a kill); return
            // whatever completed successfully below.
        }

        string standardOutput = stdoutTask.IsCompletedSuccessfully ? stdoutTask.Result : string.Empty;
        string standardError = stderrTask.IsCompletedSuccessfully ? stderrTask.Result : string.Empty;

        // A read that did not finish within the bounded drain is abandoned here.
        // Observe any eventual fault so it cannot surface as an unobserved task
        // exception, without blocking on the unfinished read.
        ObserveEventualFault(stdoutTask);
        ObserveEventualFault(stderrTask);

        return (standardOutput, standardError);
    }

    /// <summary>
    /// Ensures a read task's eventual fault is observed. If the task has already
    /// finished, any fault is observed immediately; otherwise a fault-only
    /// continuation observes it later without blocking the caller.
    /// </summary>
    private static void ObserveEventualFault(Task task)
    {
        if (task.IsCompleted)
        {
            _ = task.Exception;
            return;
        }

        _ = task.ContinueWith(
            static t => _ = t.Exception,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
    }

    /// <summary>
    /// Returns the process exit code, or <see cref="CommandResult.UnknownExitCode"/>
    /// when it is not available.
    /// </summary>
    private static int TryGetExitCode(Process process)
    {
        if (!process.HasExited)
        {
            return CommandResult.UnknownExitCode;
        }

        try
        {
            return process.ExitCode;
        }
        catch (InvalidOperationException)
        {
            return CommandResult.UnknownExitCode;
        }
    }
}
