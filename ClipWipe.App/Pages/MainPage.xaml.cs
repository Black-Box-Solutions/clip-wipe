using ClipWipe.App.PageModels;

namespace ClipWipe.App.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.RefreshClipboardCommand.CanExecute(null))
        {
            _viewModel.RefreshClipboardCommand.Execute(null);
        }
    }
}