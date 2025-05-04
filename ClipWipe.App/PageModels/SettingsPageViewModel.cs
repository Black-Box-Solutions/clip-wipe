using ClipWipe.App.Extensions;
using ClipWipe.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;

using CommunityToolkit.Mvvm.Input;

namespace ClipWipe.App.PageModels;

public partial class SettingsPageViewModel : ObservableObject
{
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

    public SettingsPageViewModel(ISettingsService settingsService, ClipboardTimerService clipboardTimerService)
    {
        _settingsService = settingsService;
        _clipboardTimerService = clipboardTimerService;

        // Initial load
        //TODO LoadSettingsAsync().SafeFireAndForget();
        Task.Run(async () => await LoadSettingsAsync());
    }

    partial void OnAutoClearEnabledChanged(bool value)
    {
        //TODO fix the async call here
        UpdateTimerServiceAsync().SafeFireAndForget();
    }
    partial void OnAutoClearIntervalMinutesChanged(int value)
    {
        //TODO fix the async call here
        UpdateTimerServiceAsync().SafeFireAndForget();
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