using Microsoft.UI.Xaml;
using Sentry;

namespace WinDroid.Studio;

/// <summary>
/// Application entry point for WinDroid Studio.
/// </summary>
public partial class App : Application
{
    private IDisposable? _sentry;
    private Window? _window;

    public App()
    {
        InitializeComponent();

        var sentryDsn = Environment.GetEnvironmentVariable("SENTRY_DSN");

        if (!string.IsNullOrWhiteSpace(sentryDsn))
        {
            _sentry = SentrySdk.Init(options =>
            {
                options.Dsn = sentryDsn;

#if DEBUG
                options.Debug = true;
#else
                options.Debug = false;
#endif

                options.AutoSessionTracking = true;
            });
        }
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Closed += OnMainWindowClosed;
        _window.Activate();
    }

    private async void OnMainWindowClosed(object sender, WindowEventArgs args)
    {
        if (_sentry is null)
        {
            return;
        }

        await SentrySdk.FlushAsync(TimeSpan.FromSeconds(2));
        _sentry.Dispose();
    }
}