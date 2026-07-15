using WinDroid.Adb.Models;

namespace WinDroid.Adb.Services;

/// <summary>
/// Runs the <c>adb devices</c> command by delegating to <see cref="ProcessRunner"/>.
/// </summary>
/// <remarks>
/// This wrapper only launches the command and returns the raw
/// <see cref="CommandResult"/>. It does not resolve the ADB path (the caller
/// supplies an already-resolved path, typically from
/// <see cref="AdbPathResolver"/>) and it does not parse the output. Parsing of
/// the device listing is intentionally left to a later stage.
/// </remarks>
public sealed class AdbDeviceService
{
    private readonly ProcessRunner _processRunner;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdbDeviceService"/> class.
    /// </summary>
    /// <param name="processRunner">The process runner used to execute ADB.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="processRunner"/> is <see langword="null"/>.
    /// </exception>
    public AdbDeviceService(ProcessRunner processRunner)
    {
        ArgumentNullException.ThrowIfNull(processRunner);

        _processRunner = processRunner;
    }

    /// <summary>
    /// Runs <c>adb devices</c> using the supplied ADB executable path and returns
    /// the raw result.
    /// </summary>
    /// <param name="adbPath">
    /// Path to the ADB executable, already resolved by the caller. It is passed
    /// through unchanged; this method does not revalidate or normalize it.
    /// </param>
    /// <param name="timeout">
    /// Optional maximum run time, forwarded to <see cref="ProcessRunner"/>.
    /// <see langword="null"/> means no timeout.
    /// </param>
    /// <param name="cancellationToken">
    /// Token forwarded to <see cref="ProcessRunner"/> to cancel execution.
    /// </param>
    /// <returns>
    /// The unmodified <see cref="CommandResult"/> produced by
    /// <see cref="ProcessRunner"/>, including its output, exit code, and timeout
    /// or cancellation state.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="adbPath"/> is <see langword="null"/>, empty, or whitespace.
    /// </exception>
    public Task<CommandResult> GetDevicesAsync(
        string adbPath,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(adbPath);

        return _processRunner.RunAsync(adbPath, ["devices"], timeout, cancellationToken);
    }
}
