using System.Diagnostics;

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

    public async Task InitializeTimerFromSettingsAsync()
    {
        // Stop existing timer if it's running
        StopTimer();

        // If auto-clear is enabled, start the timer
        if (_settingsService.AutoClearEnabled)
        {
            await StartTimerAsync(_settingsService.AutoClearIntervalMinutes);
        }
    }

    public async Task StartTimerAsync(int intervalMinutes)
    {
        try
        {
            StopTimer();

            Debug.Assert(Application.Current != null, "Application.Current is null!");

            _timer = Application.Current!.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMinutes(intervalMinutes);
            _timer.Tick += async (s, e) =>
            {
                await _clipboardService.ClearClipboardAsync();
                _settingsService.LastClearTime = DateTime.Now;
            };

            _timer.Start();

            // Use Task.CompletedTask to explicitly indicate the method is asynchronous
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            //TODO Log the exception and handle it appropriately
            Debug.WriteLine(ex);
        }
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
            Debug.WriteLine(e);
            throw;
        }
    }

    public bool IsTimerRunning => _timer?.IsRunning ?? false;
}
