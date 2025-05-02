using ClipWipe.App.Services;
using System.Windows.Input;

namespace ClipWipe.App.PageModels;

//TODO change 
public partial class MainPageViewModel : BaseViewModel
{
    private readonly IClipboardService _clipboardService;
    private readonly ISettingsService _settingsService;
    private readonly ClipboardTimerService _clipboardTimerService;

    private string _clipboardContent;
    private bool _hasClipboardContent;
    private DateTime? _lastClearedTime;
    private string _statusMessage;

    public string ClipboardContent
    {
        get => _clipboardContent;
        set => SetProperty(ref _clipboardContent, value);
    }

    public bool HasClipboardContent
    {
        get => _hasClipboardContent;
        set => SetProperty(ref _hasClipboardContent, value);
    }

    public DateTime? LastClearedTime
    {
        get => _lastClearedTime;
        set => SetProperty(ref _lastClearedTime, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    //TODO change these to use RelayCommand per Claude's suggestion
    public ICommand RefreshClipboardCommand { get; }
    public ICommand ClearClipboardCommand { get; }

    public MainPageViewModel(IClipboardService clipboardService, ISettingsService settingsService,
        ClipboardTimerService clipboardTimerService)
    {
        _clipboardService = clipboardService;
        _settingsService = settingsService;
        _clipboardTimerService = clipboardTimerService;

        RefreshClipboardCommand = new Command(RefreshClipboard);
        ClearClipboardCommand = new Command(ClearClipboard);

        RefreshClipboard();
    }

    private void RefreshClipboard()
    {
        try
        {
            HasClipboardContent = _clipboardService.HasClipboardContent();

            if (HasClipboardContent)
            {
                ClipboardContent = _clipboardService.GetClipboardContent();
                StatusMessage = "Clipboard content loaded successfully.";
            }
            else
            {
                ClipboardContent = string.Empty;
                StatusMessage = "Clipboard is empty.";
            }

            LastClearedTime = _settingsService.LastClearTime;
        }
        catch (Exception ex)
        {
            //TODO: Log the exception and handle it appropriately
            StatusMessage = $"Error: {ex.Message}";
        }
    }

    private void ClearClipboard()
    {
        try
        {
            _clipboardService.ClearClipboard();
            _settingsService.LastClearTime = DateTime.Now;

            RefreshClipboard();

            StatusMessage = "Clipboard cleared successfully.";
        }
        catch (Exception ex)
        {
            //TODO: Log the exception and handle it appropriately
            StatusMessage = $"Error clearing clipboard: {ex.Message}";
        }
    }
}