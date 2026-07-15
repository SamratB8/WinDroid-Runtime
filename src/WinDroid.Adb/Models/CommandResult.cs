namespace WinDroid.Adb.Models;

/// <summary>
/// Represents the outcome of running an external process.
/// </summary>
/// <remarks>
/// Exactly one high-level outcome applies to any result:
/// <list type="bullet">
///   <item>
///     Normal completion — <see cref="Started"/> is <see langword="true"/>,
///     <see cref="TimedOut"/> and <see cref="Cancelled"/> are
///     <see langword="false"/>, and <see cref="ExitCode"/> is the process exit
///     code (which may be non-zero).
///   </item>
///   <item>
///     Timeout — <see cref="Started"/> is <see langword="true"/> and
///     <see cref="TimedOut"/> is <see langword="true"/>.
///   </item>
///   <item>
///     Caller cancellation — <see cref="Cancelled"/> is <see langword="true"/>.
///     This may occur before the process is started (the caller token was
///     already cancelled, so <see cref="Started"/> is <see langword="false"/>)
///     or after it is started (<see cref="Started"/> is <see langword="true"/>).
///   </item>
///   <item>
///     Startup failure — <see cref="Started"/> is <see langword="false"/> and
///     <see cref="ErrorMessage"/> is non-empty.
///   </item>
/// </list>
/// Use the static factory methods to construct values that honour these
/// invariants.
/// </remarks>
public sealed class CommandResult
{
    /// <summary>
    /// Sentinel exit code used when the real process exit code is unavailable,
    /// for example after a startup failure or after a process is terminated
    /// without a reportable exit code.
    /// </summary>
    public const int UnknownExitCode = -1;

    /// <summary>
    /// Gets the process exit code, or <see cref="UnknownExitCode"/> when it is
    /// unavailable.
    /// </summary>
    public int ExitCode { get; init; }

    /// <summary>
    /// Gets the captured standard output. Never <see langword="null"/>. Holds the
    /// text of a completed stream read (including output produced before a
    /// terminated process exited); if the stream did not finish draining within
    /// the bounded cleanup period, this is an empty string.
    /// </summary>
    public string StandardOutput { get; init; } = string.Empty;

    /// <summary>
    /// Gets the captured standard error. Never <see langword="null"/>. Holds the
    /// text of a completed stream read (including output produced before a
    /// terminated process exited); if the stream did not finish draining within
    /// the bounded cleanup period, this is an empty string.
    /// </summary>
    public string StandardError { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the process was started successfully.
    /// </summary>
    public bool Started { get; init; }

    /// <summary>
    /// Gets a value indicating whether the process was terminated because the
    /// timeout elapsed.
    /// </summary>
    public bool TimedOut { get; init; }

    /// <summary>
    /// Gets a value indicating whether the process was terminated because the
    /// caller cancelled the operation.
    /// </summary>
    public bool Cancelled { get; init; }

    /// <summary>
    /// Gets a concise, user-safe message describing why the process could not be
    /// started when <see cref="Started"/> is <see langword="false"/>; otherwise
    /// <see langword="null"/>. Never contains a stack trace.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Creates a result for a process that ran to completion.
    /// </summary>
    /// <param name="exitCode">The process exit code (may be non-zero).</param>
    /// <param name="standardOutput">The captured standard output.</param>
    /// <param name="standardError">The captured standard error.</param>
    public static CommandResult Completed(int exitCode, string standardOutput, string standardError)
    {
        ArgumentNullException.ThrowIfNull(standardOutput);
        ArgumentNullException.ThrowIfNull(standardError);

        return new CommandResult
        {
            Started = true,
            TimedOut = false,
            Cancelled = false,
            ExitCode = exitCode,
            StandardOutput = standardOutput,
            StandardError = standardError,
            ErrorMessage = null,
        };
    }

    /// <summary>
    /// Creates a result for a process that was terminated because the timeout
    /// elapsed.
    /// </summary>
    /// <param name="exitCode">
    /// The exit code observed after termination, or <see cref="UnknownExitCode"/>
    /// when unavailable.
    /// </param>
    /// <param name="standardOutput">Any standard output captured before termination.</param>
    /// <param name="standardError">Any standard error captured before termination.</param>
    public static CommandResult Timeout(int exitCode, string standardOutput, string standardError)
    {
        ArgumentNullException.ThrowIfNull(standardOutput);
        ArgumentNullException.ThrowIfNull(standardError);

        return new CommandResult
        {
            Started = true,
            TimedOut = true,
            Cancelled = false,
            ExitCode = exitCode,
            StandardOutput = standardOutput,
            StandardError = standardError,
            ErrorMessage = null,
        };
    }

    /// <summary>
    /// Creates a result for a process that was terminated because the caller
    /// cancelled the operation.
    /// </summary>
    /// <param name="exitCode">
    /// The exit code observed after termination, or <see cref="UnknownExitCode"/>
    /// when unavailable.
    /// </param>
    /// <param name="standardOutput">Any standard output captured before termination.</param>
    /// <param name="standardError">Any standard error captured before termination.</param>
    public static CommandResult Cancellation(int exitCode, string standardOutput, string standardError)
    {
        ArgumentNullException.ThrowIfNull(standardOutput);
        ArgumentNullException.ThrowIfNull(standardError);

        return new CommandResult
        {
            Started = true,
            TimedOut = false,
            Cancelled = true,
            ExitCode = exitCode,
            StandardOutput = standardOutput,
            StandardError = standardError,
            ErrorMessage = null,
        };
    }

    /// <summary>
    /// Creates a result for an operation that was cancelled before the process
    /// was started, because the caller's token was already cancelled.
    /// </summary>
    public static CommandResult CancellationBeforeStart()
    {
        return new CommandResult
        {
            Started = false,
            TimedOut = false,
            Cancelled = true,
            ExitCode = UnknownExitCode,
            StandardOutput = string.Empty,
            StandardError = string.Empty,
            ErrorMessage = null,
        };
    }

    /// <summary>
    /// Creates a result for a process that could not be started.
    /// </summary>
    /// <param name="errorMessage">A concise, user-safe explanation of the failure.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="errorMessage"/> is <see langword="null"/>, empty, or whitespace.
    /// </exception>
    public static CommandResult StartupFailure(string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        return new CommandResult
        {
            Started = false,
            TimedOut = false,
            Cancelled = false,
            ExitCode = UnknownExitCode,
            StandardOutput = string.Empty,
            StandardError = string.Empty,
            ErrorMessage = errorMessage,
        };
    }
}
