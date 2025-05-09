using ClipWipe.App.PageModels;
using ClipWipe.App.Services;

namespace ClipWipe.App.Pages;

public partial class MainPage : ContentPage
{
    private readonly IClipboardService _clipboardService;
    private readonly MainPageViewModel _viewModel;

    public MainPage(IClipboardService clipboardService, MainPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _clipboardService = clipboardService;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Subscribe to clipboard changes
        _clipboardService.ClipboardChanged += OnClipboardChanged;

        if (_viewModel.RefreshClipboardCommand.CanExecute(null))
        {
            _viewModel.RefreshClipboardCommand.Execute(null);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Unsubscribe from clipboard changes when the page is not visible
        _clipboardService.ClipboardChanged -= OnClipboardChanged;
    }

    private void OnClipboardChanged(object? sender, string content)
    {
        // Update the UI on the main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update ViewModel with the new clipboard content
            _viewModel.UpdateClipboardContent(content);
            _viewModel.StatusMessage = "Clipboard updated automatically";
        });
    }
}