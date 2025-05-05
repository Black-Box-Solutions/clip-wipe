using ClipWipe.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClipWipe.App.PageModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly IClipboardService _clipboardService;
    private readonly ISettingsService _settingsService;

    //TODO do we need this? I don't think the implementation is done here
    private readonly ClipboardTimerService _clipboardTimerService;

    [ObservableProperty]
    public partial string? ClipboardContent { get; set; }

    [ObservableProperty]
    public partial bool HasClipboardContent { get; set; }

    [ObservableProperty]
    public partial DateTimeOffset? LastClearedTime { get; set; }

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = "Ready to clear clipboard.";

    public MainPageViewModel(
        IClipboardService clipboardService,
        ISettingsService settingsService,
        ClipboardTimerService clipboardTimerService)
    {
        _clipboardService = clipboardService;
        _settingsService = settingsService;
        _clipboardTimerService = clipboardTimerService;

        // Initial refresh
        //TODO         RefreshClipboardAsync().SafeFireAndForget();
        Task.Run(async () => await RefreshClipboardAsync());
    }

    [RelayCommand]
    private async Task RefreshClipboardAsync()
    {
        try
        {
            HasClipboardContent = await _clipboardService.HasClipboardContentAsync();
            if (HasClipboardContent)
            {
                ClipboardContent = await _clipboardService.GetClipboardContentAsync();
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

    [RelayCommand]
    private async Task ClearClipboardAsync()
    {
        try
        {
            await _clipboardService.ClearClipboardAsync();
            _settingsService.LastClearTime = DateTimeOffset.UtcNow;

            await RefreshClipboardAsync();

            StatusMessage = "Clipboard cleared successfully.";
        }
        catch (Exception ex)
        {
            //TODO: Log the exception and handle it appropriately
            StatusMessage = $"Error clearing clipboard: {ex.Message}";
        }
    }
}