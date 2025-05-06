using ClipWipe.App.Extensions;
using ClipWipe.App.Services;

namespace ClipWipe.App;

public partial class App : Application
{
    private readonly ClipboardTimerService _clipboardTimerService;
    private readonly IClipboardService _clipboardService;

    public App(ClipboardTimerService clipboardTimerService, IClipboardService clipboardService)
    {
        InitializeComponent();

        _clipboardTimerService = clipboardTimerService;
        _clipboardService = clipboardService;
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
        _clipboardTimerService.InitializeTimerFromSettingsAsync().SafeFireAndForget();
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        // App going to background
        _clipboardService.StopListening();
        _clipboardTimerService.SaveSettingsAsync().SafeFireAndForget();
    }

    protected override void OnResume()
    {
        base.OnResume();
        _clipboardService.StartListening();
    }
}