using AsyncAwaitBestPractices;
using ClipWipe.App.Services;

using Microsoft.Extensions.Logging;

namespace ClipWipe.App;

public partial class App : Application
{
    private readonly ILogger<App> _logger;
    private readonly ClipboardTimerService _clipboardTimerService;
    private readonly IClipboardService _clipboardService;

    public App(ClipboardTimerService clipboardTimerService, IClipboardService clipboardService, ILogger<App> logger)
    {
        InitializeComponent();

        _clipboardTimerService = clipboardTimerService;
        _clipboardService = clipboardService;
        _logger = logger;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = new Window(new AppShell());

        // Handle window lifecycle events
        window.Destroying += (_, _) =>
        {
            _clipboardService.StopListening();
            _clipboardService.Dispose();
        };

        return window;
    }

    protected override void OnStart()
    {
        base.OnStart();

        _clipboardService.StartListening();

        // Start timer service when app starts
        _clipboardTimerService.InitializeTimerFromSettingsAsync().SafeFireAndForget(ex =>
        {
            // Handle any exceptions that occur during initialization
            _logger.LogError(ex, "Error initializing timer");
        });
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        // App going to background
        _clipboardService.StopListening();
        _clipboardTimerService.SaveSettingsAsync().SafeFireAndForget(ex =>
        {
            _logger.LogError(ex, "Error saving settings during sleep");
        });
    }

    protected override void OnResume()
    {
        base.OnResume();
        _clipboardService.StartListening();
        _clipboardTimerService.InitializeTimerFromSettingsAsync().SafeFireAndForget(ex =>
        {
            _logger.LogError(ex, "Error re-initializing timer on resume");
        });
    }
}