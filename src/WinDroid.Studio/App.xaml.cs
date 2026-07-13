using Microsoft.UI.Xaml;
using Sentry;

namespace WinDroid.Studio;

/// <summary>
/// Application entry point for WinDroid Studio.
/// </summary>
public partial class App : Application
{
    private readonly IDisposable _sentry;
    private Window? _window;

    public App()
    {
        _sentry = SentrySdk.Init(options =>
        {
            options.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");

#if DEBUG
            options.Debug = true;
#else
            options.Debug = false;
#endif

            options.AutoSessionTracking = true;
        });

        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Closed += OnMainWindowClosed;
        _window.Activate();
    }

    private async void OnMainWindowClosed(object sender, WindowEventArgs args)
    {
        await SentrySdk.FlushAsync(TimeSpan.FromSeconds(2));
        _sentry.Dispose();
    }
}