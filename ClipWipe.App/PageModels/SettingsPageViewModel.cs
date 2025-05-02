using ClipWipe.App.Extensions;
using ClipWipe.App.Services;
using System.Windows.Input;

namespace ClipWipe.App.PageModels;

public partial class SettingsPageViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly ClipboardTimerService _clipboardTimerService;

    private bool _autoClearEnabled;
    private int _autoClearIntervalMinutes;
    private bool _startOnBootEnabled;
    private string _statusMessage;
    private bool _isBusy;

    public SettingsPageViewModel(ISettingsService settingsService, ClipboardTimerService clipboardTimerService)
    {
        _settingsService = settingsService;
        _clipboardTimerService = clipboardTimerService;

        SaveSettingsCommand = new Command(async () => await SaveSettingsAsync(), () => !IsBusy);

        // Initial load
        LoadSettingsAsync().SafeFireAndForget();
    }

    public bool AutoClearEnabled
    {
        get => _autoClearEnabled;
        set
        {
            if (SetProperty(ref _autoClearEnabled, value))
            {
                _settingsService.AutoClearEnabled = value;
                UpdateTimerServiceAsync().SafeFireAndForget();
            }
        }
    }

    public int AutoClearIntervalMinutes
    {
        get => _autoClearIntervalMinutes;
        set
        {
            if (SetProperty(ref _autoClearIntervalMinutes, value))
            {
                _settingsService.AutoClearIntervalMinutes = value;
                UpdateTimerServiceAsync().SafeFireAndForget();
            }
        }
    }

    public bool StartOnBootEnabled
    {
        get => _startOnBootEnabled;
        set
        {
            if (SetProperty(ref _startOnBootEnabled, value))
            {
                _settingsService.StartOnBootEnabled = value;
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public ICommand SaveSettingsCommand { get; }

    private async Task LoadSettingsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;

        try
        {
            await _settingsService.LoadSettingsAsync();

            _autoClearEnabled = _settingsService.AutoClearEnabled;
            _autoClearIntervalMinutes = _settingsService.AutoClearIntervalMinutes;
            _startOnBootEnabled = _settingsService.StartOnBootEnabled;
        }
        catch (Exception ex)
        {
            //TODO Log the exception and handle it appropriately
            StatusMessage = $"Error loading settings: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveSettingsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;

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
        finally
        {
            IsBusy = false;
        }
    }

    private async Task UpdateTimerServiceAsync()
    {
        try
        {
            if (_autoClearEnabled)
            {
                await _clipboardTimerService.StartTimerAsync(_autoClearIntervalMinutes);
                StatusMessage = $"Auto-clear timer set for {_autoClearIntervalMinutes} minutes.";
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