namespace ClipWipe.App.Services;

public class ClipboardTimerService
{
    private readonly IClipboardService _clipboardService;
    private readonly ISettingsService _settingsService;
    private IDispatcherTimer? _timer;

    public ClipboardTimerService(IClipboardService clipboardService, ISettingsService settingsService)
    {
        _clipboardService = clipboardService;
        _settingsService = settingsService;
    }

    public void InitializeTimerFromSettings()
    {
        // Stop existing timer if it's running
        StopTimer();

        // If auto-clear is enabled, start the timer
        if (_settingsService.AutoClearEnabled)
        {
            StartTimer(_settingsService.AutoClearIntervalMinutes);
        }
    }

    public void StartTimer(int intervalMinutes)
    {
        StopTimer();

        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMinutes(intervalMinutes);
        _timer.Tick += (s, e) =>
        {
            _clipboardService.ClearClipboard();
            _settingsService.LastClearTime = DateTime.Now;
        };
        _timer.Start();
    }

    public void StopTimer()
    {
        if (_timer?.IsRunning == true)
        {
            _timer.Stop();
        }
    }

    public async Task SaveSettingsAsync()
    {
        try
        {
            await _settingsService.SaveSettingsAsync();
        }
        catch (Exception e)
        {
            //TODO Log the exception and handle it appropriately
            Console.WriteLine(e);
            throw;
        }
    }

    public bool IsTimerRunning => _timer?.IsRunning ?? false;
}
