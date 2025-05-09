using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ClipWipe.App.Services;
public sealed partial class ClipboardTimerService : IDisposable
{
    private static readonly Action<ILogger, Exception?> _logMainPageNullWarning =
        LoggerMessage.Define(LogLevel.Warning, new EventId(0, nameof(OnTimerTick)), "MainPage is null. Cannot display alert.");

    private static readonly Action<ILogger, string, string, Exception> _logErrorClearingClipboard =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(1, nameof(OnTimerTick)), "Error clearing clipboard in {Method}: {ExceptionMessage}");

    private readonly IClipboardService _clipboardService;
    private readonly ISettingsService _settingsService;
    private readonly ILogger<ClipboardTimerService> _logger;
    private IDispatcherTimer? _timer;
    private bool _isDisposed;

    public bool IsTimerRunning => _timer?.IsRunning ?? false;

    public ClipboardTimerService(ILogger<ClipboardTimerService> logger, IClipboardService clipboardService, ISettingsService settingsService)
    {
        _logger = logger;
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

            Debug.Assert(Application.Current is not null, "Application.Current is null!");

            _timer = Application.Current!.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMinutes(intervalMinutes);
            _timer.Tick += OnTimerTick;

            _timer.Start();

            // Use Task.CompletedTask to explicitly indicate the method is asynchronous
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            //TODO Log the exception and handle it appropriately
            _logger.LogError(ex, "An error occurred while starting the timer.");
            Debug.WriteLine(ex);
        }
    }

    private async void OnTimerTick(object? sender, EventArgs e)
    {
        try
        {
            await _clipboardService.ClearClipboardAsync();
            _settingsService.LastClearTime = DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            _logErrorClearingClipboard(_logger, nameof(OnTimerTick), "An error occurred while clearing the clipboard.", ex);

            Application? currentApplication = Application.Current;
            if (currentApplication?.Dispatcher is not null)
            {
                await currentApplication.Dispatcher.DispatchAsync(async () =>
                {
                    if (currentApplication.Windows.Count > 0)
                    {
                        Page? mainPage = currentApplication.Windows[0].Page;
                        if (mainPage is not null)
                        {
                            await mainPage.DisplayAlert("Error", "An error occurred while clearing the clipboard.", "OK");
                        }
                        else
                        {
                            _logMainPageNullWarning(_logger, null);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No windows available to display alert.");
                    }
                });
            }
            else
            {
                Debug.WriteLine("Dispatcher is null. Cannot dispatch error handling.");
            }
        }
    }

    public void StopTimer()
    {
        if (IsTimerRunning)
        {
            if (_timer is not null)
            {
                _timer.Tick -= OnTimerTick; // Unregister the event handler
                _timer.Stop();
                _timer = null;
            }
        }
        else
        {
            Debug.WriteLine("Attempted to stop timer, but it is not running.");
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            // Ensures the timer is stopped and event handlers are unregistered
            StopTimer();
        }

        _isDisposed = true;
    }
}
