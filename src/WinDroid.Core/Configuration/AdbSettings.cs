namespace WinDroid.Core.Configuration;

/// <summary>
/// Represents user-configurable settings used by future ADB services.
/// </summary>
public sealed class AdbSettings
{
    /// <summary>
    /// Optional user-specified path to the ADB executable. When not set, a
    /// future service is expected to fall back to its own default resolution.
    /// The value is stored as provided and is not validated or normalized here.
    /// </summary>
    public string? CustomAdbPath { get; set; }

    /// <summary>
    /// Indicates whether a future service should prefer an ADB executable
    /// distributed with WinDroid. Defaults to <see langword="false"/>.
    /// </summary>
    public bool UseBundledAdb { get; set; }
}
