using AsyncAwaitBestPractices;
using ClipWipe.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace ClipWipe.App.PageModels;

public partial class SettingsPageViewModel : ObservableObject
{
    private readonly ILogger<SettingsPageViewModel> _logger;
    private readonly ISettingsService _settingsService;
    private readonly ClipboardTimerService _clipboardTimerService;

    [ObservableProperty]
    public partial bool AutoClearEnabled { get; set; }

    [ObservableProperty]
    public partial int AutoClearIntervalMinutes { get; set; }

    [ObservableProperty]
    public partial bool StartOnBootEnabled { get; set; }

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "Ready to load settings.";

    //[ObservableProperty]
    //private bool _isBusy;

    public SettingsPageViewModel(ISettingsService settingsService, ClipboardTimerService clipboardTimerService, ILogger<SettingsPageViewModel> logger)
    {
        _settingsService = settingsService;
        _clipboardTimerService = clipboardTimerService;
        _logger = logger;

        // Initial load
        LoadSettingsAsync().SafeFireAndForget(ex => _logger.LogError(ex, "Error while loading settings"));
    }

    partial void OnAutoClearEnabledChanged(bool value)
    {
        UpdateTimerServiceAsync().SafeFireAndForget(ex => _logger.LogError(ex, "Error updating timer service"));
    }

    partial void OnAutoClearIntervalMinutesChanged(int value)
    {
        UpdateTimerServiceAsync().SafeFireAndForget(ex => _logger.LogError(ex, "Error updating timer service"));
    }

    partial void OnStartOnBootEnabledChanged(bool value)
    {
        _settingsService.StartOnBootEnabled = value;
    }

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        try
        {
            await _settingsService.LoadSettingsAsync();

            AutoClearEnabled = _settingsService.AutoClearEnabled;
            AutoClearIntervalMinutes = _settingsService.AutoClearIntervalMinutes;
            StartOnBootEnabled = _settingsService.StartOnBootEnabled;
        }
        catch (Exception ex)
        {
            //TODO Log the exception and handle it appropriately
            StatusMessage = $"Error loading settings: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        try
        {
            await _settingsService.SaveSettingsAsync();
            StatusMessage = "Settings saved successfully.";
        }
        catch (Exception ex)
        {
            //TODO Log the exception and handle it appropriately
            StatusMessage = $"Error saving settings: {ex.Message}";
        }
    }

    private async Task UpdateTimerServiceAsync()
    {
        try
        {
            if (AutoClearEnabled)
            {
                await _clipboardTimerService.StartTimerAsync(AutoClearIntervalMinutes);
                StatusMessage = $"Auto-clear timer set for {AutoClearIntervalMinutes} minutes.";
            }
            else
            {
                _clipboardTimerService.StopTimer();
                StatusMessage = "Auto-clear timer disabled.";
            }
        }
        catch (Exception ex)
        {
            //TODO Log the exception and handle it appropriately
            StatusMessage = $"Error updating timer: {ex.Message}";
        }
    }
}