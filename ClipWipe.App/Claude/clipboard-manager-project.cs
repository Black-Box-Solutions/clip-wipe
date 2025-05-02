// PROJECT: ClipboardManager
// Description: A .NET MAUI application for managing and clearing clipboard content on Android devices
// Target Platform: Android (Samsung Galaxy S25+ with OneUI)

// 1. ClipboardManager.csproj
// This is the project file that defines the MAUI application
//<Project Sdk="Microsoft.NET.Sdk">

//    <PropertyGroup>
//        <TargetFrameworks>net9.0-android</TargetFrameworks>
//        <OutputType>Exe</OutputType>
//        <RootNamespace>ClipboardManager</RootNamespace>
//        <UseMaui>true</UseMaui>
//        <SingleProject>true</SingleProject>
//        <ImplicitUsings>enable</ImplicitUsings>

//        <!-- Display name -->
//        <ApplicationTitle>Clipboard Manager</ApplicationTitle>

//        <!-- App Identifier -->
//        <ApplicationId>com.yourcompany.clipboardmanager</ApplicationId>
//        <ApplicationIdGuid>56CD2C99-8F78-4AEE-87E5-999A28C9EA92</ApplicationIdGuid>

//        <!-- Versions -->
//        <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
//        <ApplicationVersion>1</ApplicationVersion>

//        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
//    </PropertyGroup>

//    <ItemGroup>
//        <!-- App Icon -->
//        <MauiIcon Include="Resources\AppIcon\appicon.svg" ForegroundFile="Resources\AppIcon\appiconfg.svg" Color="#3498DB" />

//        <!-- Splash Screen -->
//        <MauiSplashScreen Include="Resources\Splash\splash.svg" Color="#3498DB" BaseSize="128,128" />

//        <!-- Images -->
//        <MauiImage Include="Resources\Images\*" />

//        <!-- Custom Fonts -->
//        <MauiFont Include="Resources\Fonts\*" />

//        <!-- Raw Assets (also remove the "Resources\Raw" prefix) -->
//        <MauiAsset Include="Resources\Raw\**" LogicalName="%(RecursiveDir)%(Filename)%(Extension)" />
//    </ItemGroup>

//    <ItemGroup>
//        <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
//        <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
//        <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.0-preview.1.24080.9" />
//    </ItemGroup>

//</Project>

// 2. App.xaml
// This is the XAML file for the App
//<?xml version = "1.0" encoding = "UTF-8" ?>
//<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
//             xmlns:local="clr-namespace:ClipboardManager"
//             x:Class="ClipboardManager.App">
//    <Application.Resources>
//        <ResourceDictionary>
//            <ResourceDictionary.MergedDictionaries>
//                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
//                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
//            </ResourceDictionary.MergedDictionaries>
//        </ResourceDictionary>
//    </Application.Resources>
//</Application>

// 3. App.xaml.cs
// This is the code-behind file for the App
//using ClipboardManager.Services;

//namespace ClipboardManager;

//public partial class App : Application
//{
//    private readonly ClipboardTimerService _clipboardTimerService;

//    public App(ClipboardTimerService clipboardTimerService)
//    {
//        InitializeComponent();

//        _clipboardTimerService = clipboardTimerService;

//        MainPage = new AppShell();
//    }

//    protected override async void OnStart()
//    {
//        base.OnStart();

//        // Start timer service when app starts
//        await _clipboardTimerService.InitializeTimerFromSettingsAsync();
//    }

//    protected override async void OnSleep()
//    {
//        base.OnSleep();

//        // App going to background
//        await _clipboardTimerService.SaveSettingsAsync();
//    }
//}

//// 4. AppShell.xaml
//// This is the XAML file for the App Shell
//<?xml version="1.0" encoding="UTF-8" ?>
//<Shell
//    x:Class="ClipboardManager.AppShell"
//    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
//    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
//    xmlns:local="clr-namespace:ClipboardManager"
//    xmlns:views="clr-namespace:ClipboardManager.Views"
//    Shell.FlyoutBehavior="Disabled">

//    <TabBar>
//        <Tab Title="Clipboard" Icon="clipboard.png">
//            <ShellContent
//                Title="Clipboard Manager"
//                ContentTemplate="{DataTemplate views:MainPage}"
//                Route="MainPage" />
//        </Tab>
//        <Tab Title="Settings" Icon="settings.png">
//            <ShellContent
//                Title="Settings"
//                ContentTemplate="{DataTemplate views:SettingsPage}"
//                Route="SettingsPage" />
//        </Tab>
//    </TabBar>

//</Shell>

//// 5. AppShell.xaml.cs
//// This is the code-behind file for the App Shell
//namespace ClipboardManager;

//public partial class AppShell : Shell
//{
//    public AppShell()
//    {
//        InitializeComponent();
//    }
//}

//// 6. MauiProgram.cs
//// This is the entry point for the MAUI application
//using ClipboardManager.Services;
//using ClipboardManager.ViewModels;
//using ClipboardManager.Views;
//using Microsoft.Extensions.Logging;

//namespace ClipboardManager;

//public static class MauiProgram
//{
//    public static MauiApp CreateMauiApp()
//    {
//        var builder = MauiApp.CreateBuilder();
//        builder
//            .UseMauiApp<App>()
//            .ConfigureFonts(fonts =>
//            {
//                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
//            });

//        // Register services
//        builder.Services.AddSingleton<IClipboardService, ClipboardService>();
//        builder.Services.AddSingleton<ISettingsService, SettingsService>();
//        builder.Services.AddSingleton<ClipboardTimerService>();

//        // Register ViewModels
//        builder.Services.AddTransient<MainPageViewModel>();
//        builder.Services.AddTransient<SettingsPageViewModel>();

//        // Register Views
//        builder.Services.AddTransient<MainPage>();
//        builder.Services.AddTransient<SettingsPage>();

//#if DEBUG
//        builder.Logging.AddDebug();
//#endif

//        return builder.Build();
//    }
//}

// 7. Services/IClipboardService.cs
// This is the interface for the clipboard service
//using System.Threading.Tasks;

//namespace ClipboardManager.Services;

//public interface IClipboardService
//{
//    Task<string> GetClipboardContentAsync();
//    Task ClearClipboardAsync();
//    Task<bool> HasClipboardContentAsync();
//    DateTime? LastCleared { get; }
//}

// 8. Services/ClipboardService.cs
// This is the partial implementation of the clipboard service
//namespace ClipboardManager.Services;

//public partial class ClipboardService : IClipboardService
//{
//    // This partial class will be implemented differently for each platform
//    // We'll define the common properties here

//    public DateTime? LastCleared { get; protected set; }

//    // The platform-specific implementations will be in Platform folders
//}

// 9. Services/ISettingsService.cs
// This is the interface for the settings service
namespace ClipboardManager.Services;

public interface ISettingsService
{
    bool AutoClearEnabled { get; set; }
    int AutoClearIntervalMinutes { get; set; }
    bool StartOnBootEnabled { get; set; }
    DateTime? LastClearTime { get; set; }

    Task SaveSettingsAsync();
    Task LoadSettingsAsync();
}

// 10. Services/SettingsService.cs
// This is the implementation of the settings service
//using System.Threading.Tasks;

//namespace ClipboardManager.Services;

//public class SettingsService : ISettingsService
//{
//    private const string AutoClearEnabledKey = "auto_clear_enabled";
//    private const string AutoClearIntervalKey = "auto_clear_interval_minutes";
//    private const string StartOnBootEnabledKey = "start_on_boot_enabled";
//    private const string LastClearTimeKey = "last_clear_time";

//    private bool _autoClearEnabled;
//    private int _autoClearIntervalMinutes;
//    private bool _startOnBootEnabled;
//    private DateTime? _lastClearTime;

//    public bool AutoClearEnabled
//    {
//        get => _autoClearEnabled;
//        set
//        {
//            _autoClearEnabled = value;
//            Preferences.Default.Set(AutoClearEnabledKey, value);
//        }
//    }

//    public int AutoClearIntervalMinutes
//    {
//        get => _autoClearIntervalMinutes;
//        set
//        {
//            _autoClearIntervalMinutes = value;
//            Preferences.Default.Set(AutoClearIntervalKey, value);
//        }
//    }

//    public bool StartOnBootEnabled
//    {
//        get => _startOnBootEnabled;
//        set
//        {
//            _startOnBootEnabled = value;
//            Preferences.Default.Set(StartOnBootEnabledKey, value);
//        }
//    }

//    public DateTime? LastClearTime
//    {
//        get => _lastClearTime;
//        set
//        {
//            _lastClearTime = value;
//            if (value.HasValue)
//            {
//                Preferences.Default.Set(LastClearTimeKey, value.Value.ToString("o"));
//            }
//            else
//            {
//                Preferences.Default.Remove(LastClearTimeKey);
//            }
//        }
//    }

//    public SettingsService()
//    {
//        LoadSettingsAsync().ConfigureAwait(false);
//    }

//    public async Task SaveSettingsAsync()
//    {
//        await Task.Run(() =>
//        {
//            Preferences.Default.Set(AutoClearEnabledKey, _autoClearEnabled);
//            Preferences.Default.Set(AutoClearIntervalKey, _autoClearIntervalMinutes);
//            Preferences.Default.Set(StartOnBootEnabledKey, _startOnBootEnabled);

//            if (_lastClearTime.HasValue)
//            {
//                Preferences.Default.Set(LastClearTimeKey, _lastClearTime.Value.ToString("o"));
//            }
//        });
//    }

//    public async Task LoadSettingsAsync()
//    {
//        await Task.Run(() =>
//        {
//            _autoClearEnabled = Preferences.Default.Get(AutoClearEnabledKey, false);
//            _autoClearIntervalMinutes = Preferences.Default.Get(AutoClearIntervalKey, 30); // Default 30 minutes
//            _startOnBootEnabled = Preferences.Default.Get(StartOnBootEnabledKey, false);

//            string lastClearTimeStr = Preferences.Default.Get(LastClearTimeKey, string.Empty);
//            if (!string.IsNullOrEmpty(lastClearTimeStr) && DateTime.TryParse(lastClearTimeStr, out DateTime lastClearTime))
//            {
//                _lastClearTime = lastClearTime;
//            }
//            else
//            {
//                _lastClearTime = null;
//            }
//        });
//    }
//}

// 11. Services/ClipboardTimerService.cs
// This is the implementation of the clipboard timer service
//using System.Threading.Tasks;

//namespace ClipboardManager.Services;

//public class ClipboardTimerService
//{
//    private readonly IClipboardService _clipboardService;
//    private readonly ISettingsService _settingsService;
//    private IDispatcherTimer _timer;

//    public ClipboardTimerService(IClipboardService clipboardService, ISettingsService settingsService)
//    {
//        _clipboardService = clipboardService;
//        _settingsService = settingsService;
//    }

//    public async Task InitializeTimerFromSettingsAsync()
//    {
//        // Stop existing timer if it's running
//        StopTimer();

//        // If auto-clear is enabled, start the timer
//        if (_settingsService.AutoClearEnabled)
//        {
//            await StartTimerAsync(_settingsService.AutoClearIntervalMinutes);
//        }
//    }

//    public async Task StartTimerAsync(int intervalMinutes)
//    {
//        StopTimer();

//        _timer = Application.Current.Dispatcher.CreateTimer();
//        _timer.Interval = TimeSpan.FromMinutes(intervalMinutes);
//        _timer.Tick += async (s, e) =>
//        {
//            await _clipboardService.ClearClipboardAsync();
//            _settingsService.LastClearTime = DateTime.Now;
//        };

//        _timer.Start();
//    }

//    public void StopTimer()
//    {
//        if (_timer != null && _timer.IsRunning)
//        {
//            _timer.Stop();
//        }
//    }

//    public async Task SaveSettingsAsync()
//    {
//        await _settingsService.SaveSettingsAsync();
//    }

//    public bool IsTimerRunning => _timer?.IsRunning ?? false;
//}

// 12. ViewModels/BaseViewModel.cs
// This is the base view model for MVVM pattern
//using System.ComponentModel;
//using System.Runtime.CompilerServices;

//namespace ClipboardManager.ViewModels;

//public class BaseViewModel : INotifyPropertyChanged
//{
//    public event PropertyChangedEventHandler PropertyChanged;

//    protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
//    {
//        if (EqualityComparer<T>.Default.Equals(backingField, value))
//        {
//            return false;
//        }

//        backingField = value;
//        OnPropertyChanged(propertyName);
//        return true;
//    }

//    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}

// 13. ViewModels/MainPageViewModel.cs
// This is the view model for the main page
//using System.Threading.Tasks;
//using System.Windows.Input;
//using ClipboardManager.Services;

//namespace ClipboardManager.ViewModels;

//public class MainPageViewModel : BaseViewModel
//{
//    private readonly IClipboardService _clipboardService;
//    private readonly ISettingsService _settingsService;
//    private readonly ClipboardTimerService _clipboardTimerService;

//    private string _clipboardContent;
//    private bool _hasClipboardContent;
//    private DateTime? _lastClearedTime;
//    private string _statusMessage;
//    private bool _isBusy;

//    public string ClipboardContent
//    {
//        get => _clipboardContent;
//        set => SetProperty(ref _clipboardContent, value);
//    }

//    public bool HasClipboardContent
//    {
//        get => _hasClipboardContent;
//        set => SetProperty(ref _hasClipboardContent, value);
//    }

//    public DateTime? LastClearedTime
//    {
//        get => _lastClearedTime;
//        set => SetProperty(ref _lastClearedTime, value);
//    }

//    public string StatusMessage
//    {
//        get => _statusMessage;
//        set => SetProperty(ref _statusMessage, value);
//    }

//    public bool IsBusy
//    {
//        get => _isBusy;
//        set => SetProperty(ref _isBusy, value);
//    }

//    public ICommand RefreshClipboardCommand { get; }
//    public ICommand ClearClipboardCommand { get; }

//    public MainPageViewModel(
//        IClipboardService clipboardService,
//        ISettingsService settingsService,
//        ClipboardTimerService clipboardTimerService)
//    {
//        _clipboardService = clipboardService;
//        _settingsService = settingsService;
//        _clipboardTimerService = clipboardTimerService;

//        RefreshClipboardCommand = new Command(async () => await RefreshClipboardAsync(), () => !IsBusy);
//        ClearClipboardCommand = new Command(async () => await ClearClipboardAsync(), () => !IsBusy);

//        // Initial refresh
//        Task.Run(async () => await RefreshClipboardAsync());
//    }

//    private async Task RefreshClipboardAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;

//            HasClipboardContent = await _clipboardService.HasClipboardContentAsync();

//            if (HasClipboardContent)
//            {
//                ClipboardContent = await _clipboardService.GetClipboardContentAsync();
//                StatusMessage = "Clipboard content loaded successfully.";
//            }
//            else
//            {
//                ClipboardContent = string.Empty;
//                StatusMessage = "Clipboard is empty.";
//            }

//            LastClearedTime = _settingsService.LastClearTime;
//        }
//        catch (Exception ex)
//        {
//            StatusMessage = $"Error: {ex.Message}";
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    private async Task ClearClipboardAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;

//            await _clipboardService.ClearClipboardAsync();
//            _settingsService.LastClearTime = DateTime.Now;

//            await RefreshClipboardAsync();

//            StatusMessage = "Clipboard cleared successfully.";
//        }
//        catch (Exception ex)
//        {
//            StatusMessage = $"Error clearing clipboard: {ex.Message}";
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }
//}

// 14. ViewModels/SettingsPageViewModel.cs
// This is the view model for the settings page
//using System.Threading.Tasks;
//using System.Windows.Input;
//using ClipboardManager.Services;

//namespace ClipboardManager.ViewModels;

//public class SettingsPageViewModel : BaseViewModel
//{
//    private readonly ISettingsService _settingsService;
//    private readonly ClipboardTimerService _clipboardTimerService;

//    private bool _autoClearEnabled;
//    private int _autoClearIntervalMinutes;
//    private bool _startOnBootEnabled;
//    private string _statusMessage;
//    private bool _isBusy;

//    public bool AutoClearEnabled
//    {
//        get => _autoClearEnabled;
//        set
//        {
//            if (SetProperty(ref _autoClearEnabled, value))
//            {
//                _settingsService.AutoClearEnabled = value;
//                Task.Run(async () => await UpdateTimerServiceAsync());
//            }
//        }
//    }

//    public int AutoClearIntervalMinutes
//    {
//        get => _autoClearIntervalMinutes;
//        set
//        {
//            if (SetProperty(ref _autoClearIntervalMinutes, value))
//            {
//                _settingsService.AutoClearIntervalMinutes = value;
//                Task.Run(async () => await UpdateTimerServiceAsync());
//            }
//        }
//    }

//    public bool StartOnBootEnabled
//    {
//        get => _startOnBootEnabled;
//        set
//        {
//            if (SetProperty(ref _startOnBootEnabled, value))
//            {
//                _settingsService.StartOnBootEnabled = value;
//            }
//        }
//    }

//    public string StatusMessage
//    {
//        get => _statusMessage;
//        set => SetProperty(ref _statusMessage, value);
//    }

//    public bool IsBusy
//    {
//        get => _isBusy;
//        set => SetProperty(ref _isBusy, value);
//    }

//    public ICommand SaveSettingsCommand { get; }

//    public SettingsPageViewModel(
//        ISettingsService settingsService,
//        ClipboardTimerService clipboardTimerService)
//    {
//        _settingsService = settingsService;
//        _clipboardTimerService = clipboardTimerService;

//        SaveSettingsCommand = new Command(async () => await SaveSettingsAsync(), () => !IsBusy);

//        // Initial load
//        Task.Run(async () => await LoadSettingsAsync());
//    }

//    private async Task LoadSettingsAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;

//            await _settingsService.LoadSettingsAsync();

//            _autoClearEnabled = _settingsService.AutoClearEnabled;
//            _autoClearIntervalMinutes = _settingsService.AutoClearIntervalMinutes;
//            _startOnBootEnabled = _settingsService.StartOnBootEnabled;
//        }
//        catch (Exception ex)
//        {
//            StatusMessage = $"Error loading settings: {ex.Message}";
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    private async Task SaveSettingsAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;

//            await _settingsService.SaveSettingsAsync();
//            StatusMessage = "Settings saved successfully.";
//        }
//        catch (Exception ex)
//        {
//            StatusMessage = $"Error saving settings: {ex.Message}";
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    private async Task UpdateTimerServiceAsync()
//    {
//        if (_autoClearEnabled)
//        {
//            await _clipboardTimerService.StartTimerAsync(_autoClearIntervalMinutes);
//            StatusMessage = $"Auto-clear timer set for {_autoClearIntervalMinutes} minutes.";
//        }
//        else
//        {
//            _clipboardTimerService.StopTimer();
//            StatusMessage = "Auto-clear timer disabled.";
//        }
//    }
//}

// 15. Views/MainPage.xaml
// This is the XAML file for the main page
//<? xml version="1.0" encoding="utf-8" ?>
//<ContentPage xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
//             xmlns:viewmodels="clr-namespace:ClipboardManager.ViewModels"
//             x:Class="ClipboardManager.Views.MainPage"
//             x:DataType="viewmodels:MainPageViewModel"
//             Title="Clipboard Manager">

//    <ScrollView>
//        <VerticalStackLayout Padding = "25" Spacing="20">
//            <Label Text = "Clipboard Manager" FontSize="24" FontAttributes="Bold" HorizontalOptions="Center" />

//            <Frame BorderColor = "LightGray" Padding="15" HasShadow="True">
//                <VerticalStackLayout Spacing = "10" >
//                    < Label Text="Current Clipboard Content" FontSize="16" FontAttributes="Bold" />

//                    <Border StrokeShape = "RoundRectangle 10,10,10,10"
//                            Stroke="LightGray"
//                            StrokeThickness="1"
//                            Padding="10">
//                        <ScrollView MaximumHeightRequest = "150" >
//                            < Label Text="{Binding ClipboardContent}" 
//                                   IsVisible="{Binding HasClipboardContent}"
//                                   LineBreakMode="WordWrap"/>
//                        </ScrollView>
//                    </Border>

//                    <Label Text = "Clipboard is empty"
//                           IsVisible="{Binding HasClipboardContent, Converter={StaticResource InvertedBoolConverter}}"
//                           HorizontalOptions="Center"
//                           TextColor="Gray"/>

//                    <ActivityIndicator IsRunning = "{Binding IsBusy}"
//                                       IsVisible="{Binding IsBusy}"
//                                       HorizontalOptions="Center" />

//                    <Button Text = "Refresh Clipboard"
//                            Command="{Binding RefreshClipboardCommand}"
//                            IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}"
//                            BackgroundColor="#3498db"
//                            TextColor="White"
//                            Margin="0,10,0,0"/>
//                </VerticalStackLayout>
//            </Frame>

//            <Frame BorderColor = "LightGray" Padding="15" HasShadow="True">
//                <VerticalStackLayout Spacing = "10" >
//                    < Label Text="Clipboard Actions" FontSize="16" FontAttributes="Bold" />

//                    <Button Text = "Clear Clipboard"
//                            Command="{Binding ClearClipboardCommand}"
//                            IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}"
//                            BackgroundColor="#e74c3c"
//                            TextColor="White"/>

//                    <VerticalStackLayout IsVisible = "{Binding LastClearedTime, Converter={StaticResource NullToBoolConverter}}" >
//                        < Label Text="Last Cleared:" FontAttributes="Bold" />
//                        <Label Text = "{Binding LastClearedTime, StringFormat='{0:MM/dd/yyyy hh:mm tt}'}" />
//                    </ VerticalStackLayout >


//                    < Label Text="{Binding StatusMessage}" 
//                           TextColor="DimGray"
//                           HorizontalOptions="Center"
//                           Margin="0,10,0,0"/>
//                </VerticalStackLayout>
//            </Frame>

//            <Label Text = "Press 'Refresh Clipboard' to check current clipboard content. Press 'Clear Clipboard' to clear clipboard content immediately."
//                   TextColor="Gray"
//                   HorizontalOptions="Center"
//                   HorizontalTextAlignment="Center"
//                   Margin="0,10,0,0"/>

//        </VerticalStackLayout>
//    </ScrollView>

//</ContentPage>

// 16. Views/MainPage.xaml.cs
// This is the code-behind file for the main page
//using ClipboardManager.ViewModels;

//namespace ClipboardManager.Views;

//public partial class MainPage : ContentPage
//{
//    private readonly MainPageViewModel _viewModel;

//    public MainPage(MainPageViewModel viewModel)
//    {
//        InitializeComponent();
//        _viewModel = viewModel;
//        BindingContext = _viewModel;
//    }

//    protected override void OnAppearing()
//    {
//        base.OnAppearing();
//        _viewModel.RefreshClipboardCommand.Execute(null);
//    }
//}

// 17. Views/SettingsPage.xaml
// This is the XAML file for the settings page
//<? xml version="1.0" encoding="utf-8" ?>
//<ContentPage xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
//             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
//             xmlns:viewmodels="clr-namespace:ClipboardManager.ViewModels"
//             x:Class="ClipboardManager.Views.SettingsPage"
//             x:DataType="viewmodels:SettingsPageViewModel"
//             Title="Settings">

//    <ScrollView>
//        <VerticalStackLayout Padding = "25" Spacing="20">
//            <Label Text = "Clipboard Manager Settings" FontSize="24" FontAttributes="Bold" HorizontalOptions="Center" />

//            <Frame BorderColor = "LightGray" Padding="15" HasShadow="True">
//                <VerticalStackLayout Spacing = "15" >
//                    < Label Text="Auto-Clear Settings" FontSize="16" FontAttributes="Bold" />

//                    <HorizontalStackLayout Spacing = "10" >
//                        < CheckBox IsChecked="{Binding AutoClearEnabled}" 
//                                  IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}" />
//                        <Label Text = "Enable Auto-Clear Clipboard" VerticalOptions="Center" />
//                    </HorizontalStackLayout>

//                    <VerticalStackLayout IsVisible = "{Binding AutoClearEnabled}" >
//                        < Label Text="Clear Interval (in minutes):" Margin="0,0,0,5" />
//                        <HorizontalStackLayout Spacing = "15" >
//                            < Slider Value="{Binding AutoClearIntervalMinutes}"
//                                    Minimum="1"
//                                    Maximum="120"
//                                    MinimumTrackColor="#3498db"
//                                    MaximumTrackColor="LightGray"
//                                    ThumbColor="#3498db"
//                                    IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}"
//                                    HorizontalOptions="FillAndExpand"/>
//                            <Label Text = "{Binding AutoClearIntervalMinutes, StringFormat='{0} min'}"
//                                   VerticalOptions="Center"
//                                   WidthRequest="50"/>
//                        </HorizontalStackLayout>
//                    </VerticalStackLayout>
//                </VerticalStackLayout>
//            </Frame>

//            <Frame BorderColor = "LightGray" Padding="15" HasShadow="True">
//                <VerticalStackLayout Spacing = "15" >
//                    < Label Text="System Settings" FontSize="16" FontAttributes="Bold" />

//                    <HorizontalStackLayout Spacing = "10" >
//                        < CheckBox IsChecked="{Binding StartOnBootEnabled}"
//                                  IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}" />
//                        <Label Text = "Start on device boot" VerticalOptions="Center" />
//                    </HorizontalStackLayout>

//                    <ActivityIndicator IsRunning = "{Binding IsBusy}"
//                                       IsVisible="{Binding IsBusy}"
//                                       HorizontalOptions="Center" />

//                    <Button Text = "Save Settings"
//                            Command="{Binding SaveSettingsCommand}"
//                            IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}"
//                            BackgroundColor="#2ecc71"
//                            TextColor="White"
//                            Margin="0,10,0,0"/>

//                    <Label Text = "{Binding StatusMessage}"
//                           TextColor="DimGray"
//                           HorizontalOptions="Center"/>
//                </VerticalStackLayout>
//            </Frame>

//            <Label Text = "Auto-clear will automatically clear your clipboard after the specified interval. Start on device boot will launch the app automatically when your device starts."
//                   TextColor="Gray"
//                   HorizontalOptions="Center"
//                   HorizontalTextAlignment="Center"
//                   Margin="0,10,0,0"/>

//        </VerticalStackLayout>
//    </ScrollView>

//</ContentPage>

// 18. Views/SettingsPage.xaml.cs
// This is the code-behind file for the settings page
//using ClipboardManager.ViewModels;

//namespace ClipboardManager.Views;

//public partial class SettingsPage : ContentPage
//{
//    private readonly SettingsPageViewModel _viewModel;

//    public SettingsPage(SettingsPageViewModel viewModel)
//    {
//        InitializeComponent();
//        _viewModel = viewModel;
//        BindingContext = _viewModel;
//    }
//}

// 19. Platforms/Android/ClipboardService.cs
// This is the Android-specific implementation of the clipboard service
//using System.Threading.Tasks;
//using Android.Content;
//using ClipboardManager = Android.Content.ClipboardManager;

//namespace ClipboardManager.Services;

//public partial class ClipboardService : IClipboardService
//{
//    private readonly Context _context;

//    public ClipboardService()
//    {
//        _context = Android.App.Application.Context;
//    }

//    public async Task<string> GetClipboardContentAsync()
//    {
//        return await Task.Run(() => {
//            var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;

//            if (clipboardManager != null && clipboardManager.HasPrimaryClip)
//            {
//                var clipData = clipboardManager.PrimaryClip;
//                if (clipData != null && clipData.ItemCount > 0)
//                {
//                    return clipData.GetItemAt(0)?.Text ?? string.Empty;
//                }
//            }

//            return string.Empty;
//        });
//    }

//    public async Task ClearClipboardAsync()
//    {
//        await Task.Run(() => {
//            var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;

//            if (clipboardManager != null)
//            {
//                var clipData = ClipData.NewPlainText("", "");
//                clipboardManager.PrimaryClip = clipData;
//                LastCleared = DateTime.Now;
//            }
//        });
//    }

//    public async Task<bool> HasClipboardContentAsync()
//    {
//        return await Task.Run(() => {
//            var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;

//            return clipboardManager != null && 
//                   clipboardManager.HasPrimaryClip && 
//                   clipboardManager.PrimaryClip?.ItemCount > 0 &&
//                   !string.IsNullOrEmpty(clipboardManager.PrimaryClip?.GetItemAt(0)?.Text);
//        });
//    }
//}

// 20. Platforms/Android/AndroidManifest.xml
// This is the Android manifest file
//<? xml version="1.0" encoding="utf-8"?>
//<manifest xmlns:android="http://schemas.android.com/apk/res/android">
//    <application android:allowBackup="true" android:icon="@mipmap/appicon" android:roundIcon="@mipmap/appicon_round" android:supportsRtl="true"></application>
//    <uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED" />
//    <uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
//</manifest>

// 21. Platforms/Android/BootReceiver.cs
// This is the boot receiver for starting the app on device boot
//using Android.App;
//using Android.Content;
//using ClipboardManager.Services;

//namespace ClipboardManager.Platforms.Android;

//[BroadcastReceiver(Enabled = true, Exported = true)]
//[IntentFilter(new[] { Intent.ActionBootCompleted })]
//public class BootReceiver : BroadcastReceiver
//{
//    public override void OnReceive(Context context, Intent intent)
//    {
//        if (intent.Action == Intent.ActionBootCompleted)
//        {
//            // Check if start on boot is enabled in settings
//            var preferences = context.GetSharedPreferences("ClipboardManager.Settings", FileCreationMode.Private);
//            bool startOnBootEnabled = preferences.GetBoolean("start_on_boot_enabled", false);

//            if (startOnBootEnabled)
//            {
//                var startIntent = new Intent(context, typeof(MainActivity));
//                startIntent.AddFlags(ActivityFlags.NewTask);
//                context.StartActivity(startIntent);
//            }
//        }
//    }
//}

// 22. Platforms/Android/Resources/values/colors.xml
// This is the Android colors resource file
//<? xml version="1.0" encoding="utf-8"?>
//<resources>
//    <color name = "colorPrimary" >#3498DB</color>
//    < color name="colorPrimaryDark">#2980B9</color>
//    <color name = "colorAccent" >#2ECC71</color>
//</ resources >

// 23. Platforms/Android/MainActivity.cs
// This is the main activity for Android
//using Android.App;
//using Android.Content.PM;
//using Android.OS;

//namespace ClipboardManager;

//[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
//public class MainActivity : MauiAppCompatActivity
//{
//    protected override void OnCreate(Bundle savedInstanceState)
//    {
//        base.OnCreate(savedInstanceState);
//        Platform.Init(this, savedInstanceState);
//    }

//    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
//    {
//        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
//        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
//    }
//}

// 24. Converters/InvertedBoolConverter.cs
// This is a converter that inverts a boolean value
//using System.Globalization;

//namespace ClipboardManager.Converters;

//public class InvertedBoolConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is bool boolValue)
//        {
//            return !boolValue;
//        }

//        return false;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is bool boolValue)
//        {
//            return !boolValue;
//        }

//        return true;
//    }
//}

// 25. Converters/NullToBoolConverter.cs
// This is a converter that converts null to a boolean value
//using System.Globalization;

//namespace ClipboardManager.Converters;

//public class NullToBoolConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        return value != null;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}

// 26. Resources/Styles/Colors.xaml
// This is the colors resource file
//<? xml version="1.0" encoding="UTF-8" ?>
//<? xaml-comp compile = "true" ?>
//< ResourceDictionary
//    xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
//    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

//    <Color x:Key="Primary">#3498db</Color>
//    <Color x:Key="Secondary">#2ecc71</Color>
//    <Color x:Key="Tertiary">#e74c3c</Color>
//    <Color x:Key="White">White</Color>
//    <Color x:Key="Black">Black</Color>
//    <Color x:Key="Gray100">#E1E1E1</Color>
//    <Color x:Key="Gray200">#C8C8C8</Color>
//    <Color x:Key="Gray300">#ACACAC</Color>
//    <Color x:Key="Gray400">#919191</Color>
//    <Color x:Key="Gray500">#6E6E6E</Color>
//    <Color x:Key="Gray600">#404040</Color>
//    <Color x:Key="Gray900">#212121</Color>
//    <Color x:Key="Gray950">#141414</Color>
//    <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource Primary}"/>
//    <SolidColorBrush x:Key="SecondaryBrush" Color="{StaticResource Secondary}"/>
//    <SolidColorBrush x:Key="TertiaryBrush" Color="{StaticResource Tertiary}"/>
//    <SolidColorBrush x:Key="WhiteBrush" Color="{StaticResource White}"/>
//    <SolidColorBrush x:Key="BlackBrush" Color="{StaticResource Black}"/>
//    <SolidColorBrush x:Key="Gray100Brush" Color="{StaticResource Gray100}"/>
//    <SolidColorBrush x:Key="Gray200Brush" Color="{StaticResource Gray200}"/>
//    <SolidColorBrush x:Key="Gray300Brush" Color="{StaticResource Gray300}"/>
//    <SolidColorBrush x:Key="Gray400Brush" Color="{StaticResource Gray400}"/>
//    <SolidColorBrush x:Key="Gray500Brush" Color="{StaticResource Gray500}"/>
//    <SolidColorBrush x:Key="Gray600Brush" Color="{StaticResource Gray600}"/>
//    <SolidColorBrush x:Key="Gray900Brush" Color="{StaticResource Gray900}"/>
//    <SolidColorBrush x:Key="Gray950Brush" Color="{StaticResource Gray950}"/>

//    <Color x:Key="Yellow100Accent">#F7B548</Color>
//    <Color x:Key="Yellow200Accent">#FFD590</Color>
//    <Color x:Key="Yellow300Accent">#FFE5B9</Color>
//    <Color x:Key="Cyan100Accent">#28C2D1</Color>
//    <Color x:Key="Cyan200Accent">#7BDDEF</Color>
//    <Color x:Key="Cyan300Accent">#C3F2F4</Color>
//    <Color x:Key="Blue100Accent">#3E8EED</Color>
//    <Color x:Key="Blue200Accent">#72ACF1</Color>
//    <Color x:Key="Blue300Accent">#A7CBF6</Color>

//</ResourceDictionary>

// 27. Resources/Styles/Styles.xaml
// This is the styles resource file
//<? xml version="1.0" encoding="UTF-8" ?>
//<? xaml-comp compile = "true" ?>
//< ResourceDictionary
//    xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
//    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
//    xmlns:converters="clr-namespace:ClipboardManager.Converters">

//    <Style TargetType = "ActivityIndicator" >
//        < Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource White}}" />
//    </Style>

//    <Style TargetType = "IndicatorView" >
//        < Setter Property="IndicatorColor" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray500}}" />
//        <Setter Property = "SelectedIndicatorColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource White}}" />
//    </Style>

//    <Style TargetType = "Border" >
//        < Setter Property="Stroke" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray500}}" />
//        <Setter Property = "StrokeShape" Value="Rectangle" />
//        <Setter Property = "StrokeThickness" Value="1" />
//    </Style>

//    <Style TargetType = "BoxView" >
//        < Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Gray950}, Dark={StaticResource Gray200}}" />
//    </Style>

//    <Style TargetType = "Button" >
//        < Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource White}, Dark={StaticResource White}}" />
//        <Setter Property = "BackgroundColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
//        <Setter Property = "FontFamily" Value="OpenSansRegular" />
//        <Setter Property = "FontSize" Value="14" />
//        <Setter Property = "CornerRadius" Value="8" />
//        <Setter Property = "Padding" Value="14,10" />
//    </Style>

//    <Style TargetType = "CheckBox" >
//        < Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
//        <Setter Property = "VisualStateManager.VisualStateGroups" >
//            < VisualStateGroupList >
//                < VisualStateGroup x:Name="CommonStates">
//                    <VisualState x:Name="Normal" />
//                    <VisualState x:Name="Disabled">
//                        <VisualState.Setters>
//                            <Setter Property = "Color" Value="{AppThemeBinding Light={StaticResource Gray300}, Dark={StaticResource Gray600}}" />
//                        </VisualState.Setters>
//                    </VisualState>
//                </VisualStateGroup>
//            </VisualStateGroupList>
//        </Setter>
//    </Style>

//    <Style TargetType = "Frame" >
//        < Setter Property="HasShadow" Value="False" />
//        <Setter Property = "BorderColor" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray500}}" />
//        <Setter Property = "CornerRadius" Value="10" />
//    </Style>

//    <Style TargetType = "Label" >
//        < Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource Black}, Dark={StaticResource White}}" />
//        <Setter Property = "FontFamily" Value="OpenSansRegular" />
//        <Setter Property = "FontSize" Value="14" />
//        <Setter Property = "VisualStateManager.VisualStateGroups" >
//            < VisualStateGroupList >
//                < VisualStateGroup x:Name="CommonStates">
//                    <VisualState x:Name="Normal" />
//                    <VisualState x:Name="Disabled">
//                        <VisualState.Setters>
//                            <Setter Property = "TextColor" Value="{AppThemeBinding Light={StaticResource Gray300}, Dark={StaticResource Gray600}}" />
//                        </VisualState.Setters>
//                    </VisualState>
//                </VisualStateGroup>
//            </VisualStateGroupList>
//        </Setter>
//    </Style>

//    <Style TargetType = "Slider" >
//        < Setter Property="MinimumTrackColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
//        <Setter Property = "MaximumTrackColor" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray600}}" />
//        <Setter Property = "ThumbColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
//    </Style>

//    <converters:InvertedBoolConverter x:Key="InvertedBoolConverter"/>
//    <converters:NullToBoolConverter x:Key="NullToBoolConverter"/>

//</ResourceDictionary>

// 28. Resources/AppIcon/appicon.svg
// This is the app icon SVG file
//<svg xmlns = "http://www.w3.org/2000/svg" viewBox="0 0 512 512">
//  <rect width = "512" height="512" rx="50" ry="50" fill="#3498db"/>
//  <rect x = "120" y="80" width="272" height="352" rx="20" ry="20" fill="#ffffff"/>
//  <rect x = "160" y="120" width="192" height="30" rx="5" ry="5" fill="#e74c3c"/>
//  <rect x = "160" y="170" width="192" height="30" rx="5" ry="5" fill="#f1c40f"/>
//  <rect x = "160" y="220" width="192" height="30" rx="5" ry="5" fill="#2ecc71"/>
//  <rect x = "160" y="270" width="192" height="30" rx="5" ry="5" fill="#9b59b6"/>
//  <rect x = "160" y="320" width="120" height="30" rx="5" ry="5" fill="#3498db"/>
//</svg>

// 29. Resources/AppIcon/appiconfg.svg
// This is the app icon foreground SVG file
//<svg xmlns = "http://www.w3.org/2000/svg" viewBox="0 0 512 512">
//  <rect x = "120" y="80" width="272" height="352" rx="20" ry="20" fill="#ffffff"/>
//  <rect x = "160" y="120" width="192" height="30" rx="5" ry="5" fill="#e74c3c"/>
//  <rect x = "160" y="170" width="192" height="30" rx="5" ry="5" fill="#f1c40f"/>
//  <rect x = "160" y="220" width="192" height="30" rx="5" ry="5" fill="#2ecc71"/>
//  <rect x = "160" y="270" width="192" height="30" rx="5" ry="5" fill="#9b59b6"/>
//  <rect x = "160" y="320" width="120" height="30" rx="5" ry="5" fill="#3498db"/>
//</svg>

// 30. Resources/Splash/splash.svg
// This is the splash screen SVG file
//<svg xmlns = "http://www.w3.org/2000/svg" viewBox="0 0 512 512">
//  <rect x = "120" y="80" width="272" height="352" rx="20" ry="20" fill="#ffffff"/>
//  <rect x = "160" y="120" width="192" height="30" rx="5" ry="5" fill="#e74c3c"/>
//  <rect x = "160" y="170" width="192" height="30" rx="5" ry="5" fill="#f1c40f"/>
//  <rect x = "160" y="220" width="192" height="30" rx="5" ry="5" fill="#2ecc71"/>
//  <rect x = "160" y="270" width="192" height="30" rx="5" ry="5" fill="#9b59b6"/>
//  <rect x = "160" y="320" width="120" height="30" rx="5" ry="5" fill="#3498db"/>
//</svg>


