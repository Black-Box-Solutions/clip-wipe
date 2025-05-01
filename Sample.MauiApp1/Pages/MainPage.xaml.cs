using Sample.MauiApp1.Models;
using Sample.MauiApp1.PageModels;

namespace Sample.MauiApp1.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}