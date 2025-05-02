using ClipWipe.App.Extensions;
using ClipWipe.App.Services;

namespace ClipWipe.App;

public partial class App : Application
{
    private readonly ClipboardTimerService _clipboardTimerService;

    public App(ClipboardTimerService clipboardTimerService)
    {
        InitializeComponent();

        _clipboardTimerService = clipboardTimerService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnStart()
    {
        base.OnStart();

        // Start timer service when app starts
        _clipboardTimerService.InitializeTimerFromSettingsAsync().SafeFireAndForget();
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        // App going to background
        _clipboardTimerService.SaveSettingsAsync().SafeFireAndForget();
    }
}