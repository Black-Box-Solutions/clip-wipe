// ClipboardManager Solution Structure

// 1. App.xaml
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

//// 2. App.xaml.cs
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

//    protected override void OnStart()
//    {
//        base.OnStart();

//        // Start timer service when app starts
//        _clipboardTimerService.InitializeTimerFromSettings();
//    }

//    protected override void OnSleep()
//    {
//        base.OnSleep();

//        // App going to background
//        _clipboardTimerService.SaveSettings();
//    }
//}

//// 3. AppShell.xaml
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

//// 4. AppShell.xaml.cs
//namespace ClipboardManager;

//public partial class AppShell : Shell
//{
//    public AppShell()
//    {
//        InitializeComponent();
//    }
//}

//// 5. MauiProgram.cs
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

//// 6. Services/IClipboardService.cs
//namespace ClipboardManager.Services;

//public interface IClipboardService
//{
//    string GetClipboardContent();
//    void ClearClipboard();
//    bool HasClipboardContent();
//    DateTime? LastCleared { get; }
//}

//// 7. Services/ClipboardService.cs
//namespace ClipboardManager.Services;

//public partial class ClipboardService : IClipboardService
//{
//    // This partial class will be implemented differently for each platform
//    // We'll define the common properties here

//    public DateTime? LastCleared { get; private set; }

//    // The platform-specific implementations will be in Platform folders
//}

//// 8. Services/ISettingsService.cs
//namespace ClipboardManager.Services;

//public interface ISettingsService
//{
//    bool AutoClearEnabled { get; set; }
//    int AutoClearIntervalMinutes { get; set; }
//    bool StartOnBootEnabled { get; set; }
//    DateTime? LastClearTime { get; set; }

//    void SaveSettings();
//    void LoadSettings();
//}

// 9. Services/SettingsService.cs
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
//        LoadSettings();
//    }

//    public void SaveSettings()
//    {
//        Preferences.Default.Set(AutoClearEnabledKey, _autoClearEnabled);
//        Preferences.Default.Set(AutoClearIntervalKey, _autoClearIntervalMinutes);
//        Preferences.Default.Set(StartOnBootEnabledKey, _startOnBootEnabled);

//        if (_lastClearTime.HasValue)
//        {
//            Preferences.Default.Set(LastClearTimeKey, _lastClearTime.Value.ToString("o"));
//        }
//    }

//    public void LoadSettings()
//    {
//        _autoClearEnabled = Preferences.Default.Get(AutoClearEnabledKey, false);
//        _autoClearIntervalMinutes = Preferences.Default.Get(AutoClearIntervalKey, 30); // Default 30 minutes
//        _startOnBootEnabled = Preferences.Default.Get(StartOnBootEnabledKey, false);

//        string lastClearTimeStr = Preferences.Default.Get(LastClearTimeKey, string.Empty);
//        if (!string.IsNullOrEmpty(lastClearTimeStr) && DateTime.TryParse(lastClearTimeStr, out DateTime lastClearTime))
//        {
//            _lastClearTime = lastClearTime;
//        }
//        else
//        {
//            _lastClearTime = null;
//        }
//    }
//}

//// 10. Services/ClipboardTimerService.cs
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

//    public void InitializeTimerFromSettings()
//    {
//        // Stop existing timer if it's running
//        StopTimer();

//        // If auto-clear is enabled, start the timer
//        if (_settingsService.AutoClearEnabled)
//        {
//            StartTimer(_settingsService.AutoClearIntervalMinutes);
//        }
//    }

//    public void StartTimer(int intervalMinutes)
//    {
//        StopTimer();

//        _timer = Application.Current.Dispatcher.CreateTimer();
//        _timer.Interval = TimeSpan.FromMinutes(intervalMinutes);
//        _timer.Tick += (s, e) =>
//        {
//            _clipboardService.ClearClipboard();
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

//    public void SaveSettings()
//    {
//        _settingsService.SaveSettings();
//    }

//    public bool IsTimerRunning => _timer?.IsRunning ?? false;
//}

// 11. ViewModels/BaseViewModel.cs
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

// 12. ViewModels/MainPageViewModel.cs
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

//        RefreshClipboardCommand = new Command(RefreshClipboard);
//        ClearClipboardCommand = new Command(ClearClipboard);

//        RefreshClipboard();
//    }

//    private void RefreshClipboard()
//    {
//        try
//        {
//            HasClipboardContent = _clipboardService.HasClipboardContent();

//            if (HasClipboardContent)
//            {
//                ClipboardContent = _clipboardService.GetClipboardContent();
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
//    }

//    private void ClearClipboard()
//    {
//        try
//        {
//            _clipboardService.ClearClipboard();
//            _settingsService.LastClearTime = DateTime.Now;

//            RefreshClipboard();

//            StatusMessage = "Clipboard cleared successfully.";
//        }
//        catch (Exception ex)
//        {
//            StatusMessage = $"Error clearing clipboard: {ex.Message}";
//        }
//    }
//}

// 13. ViewModels/SettingsPageViewModel.cs
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

//    public bool AutoClearEnabled
//    {
//        get => _autoClearEnabled;
//        set
//        {
//            if (SetProperty(ref _autoClearEnabled, value))
//            {
//                _settingsService.AutoClearEnabled = value;
//                UpdateTimerService();
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
//                UpdateTimerService();
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

//    public ICommand SaveSettingsCommand { get; }

//    public SettingsPageViewModel(
//        ISettingsService settingsService,
//        ClipboardTimerService clipboardTimerService)
//    {
//        _settingsService = settingsService;
//        _clipboardTimerService = clipboardTimerService;

//        SaveSettingsCommand = new Command(SaveSettings);

//        LoadSettings();
//    }

//    private void LoadSettings()
//    {
//        _autoClearEnabled = _settingsService.AutoClearEnabled;
//        _autoClearIntervalMinutes = _settingsService.AutoClearIntervalMinutes;
//        _startOnBootEnabled = _settingsService.StartOnBootEnabled;
//    }

//    private void SaveSettings()
//    {
//        try
//        {
//            _settingsService.SaveSettings();
//            StatusMessage = "Settings saved successfully.";
//        }
//        catch (Exception ex)
//        {
//            StatusMessage = $"Error saving settings: {ex.Message}";
//        }
//    }

//    private void UpdateTimerService()
//    {
//        if (_autoClearEnabled)
//        {
//            _clipboardTimerService.StartTimer(_autoClearIntervalMinutes);
//            StatusMessage = $"Auto-clear timer set for {_autoClearIntervalMinutes} minutes.";
//        }
//        else
//        {
//            _clipboardTimerService.StopTimer();
//            StatusMessage = "Auto-clear timer disabled.";
//        }
//    }
//}

// 14. Views/MainPage.xaml
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

//                    <Button Text = "Refresh Clipboard"
//                            Command="{Binding RefreshClipboardCommand}"
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

// 15. Views/MainPage.xaml.cs
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

// 16. Views/SettingsPage.xaml
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
//                        < CheckBox IsChecked="{Binding AutoClearEnabled}" />
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
//                        < CheckBox IsChecked="{Binding StartOnBootEnabled}" />
//                        <Label Text = "Start on device boot" VerticalOptions="Center" />
//                    </HorizontalStackLayout>

//                    <Button Text = "Save Settings"
//                            Command="{Binding SaveSettingsCommand}"
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

// 17. Views/SettingsPage.xaml.cs
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

// 18. Platforms/Android/ClipboardService.cs
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

//    public string GetClipboardContent()
//    {
//        var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;

//        if (clipboardManager != null && clipboardManager.HasPrimaryClip)
//        {
//            var clipData = clipboardManager.PrimaryClip;
//            if (clipData != null && clipData.ItemCount > 0)
//            {
//                return clipData.GetItemAt(0)?.Text ?? string.Empty;
//            }
//        }

//        return string.Empty;
//    }

//    public void ClearClipboard()
//    {
//        var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;

//        if (clipboardManager != null)
//        {
//            var clipData = ClipData.NewPlainText("", "");
//            clipboardManager.PrimaryClip = clipData;
//            LastCleared = DateTime.Now;
//        }
//    }

//    public bool HasClipboardContent()
//    {
//        var clipboardManager = _context.GetSystemService(Context.ClipboardService) as ClipboardManager;

//        return clipboardManager != null &&
//               clipboardManager.HasPrimaryClip &&
//               clipboardManager.PrimaryClip?.ItemCount > 0 &&
//               !string.IsNullOrEmpty(clipboardManager.PrimaryClip?.GetItemAt(0)?.Text);
//    }
//}

// 19. Platforms/Android/AndroidManifest.xml
//<? xml version="1.0" encoding="utf-8"?>
//<manifest xmlns:android="http://schemas.android.com/apk/res/android">
//    <application android:allowBackup="true" android:icon="@mipmap/appicon" android:roundIcon="@mipmap/appicon_round" android:supportsRtl="true"></application>
//    <uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED" />
//    <uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
//</manifest>

// 20. Platforms/Android/BootReceiver.cs
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

// 21. Platforms/Android/Resources/values/colors.xml
//<? xml version="1.0" encoding="utf-8"?>
//<resources>
//    <color name = "colorPrimary" >#3498DB</color>
//    < color name="colorPrimaryDark">#2980B9</color>
//    <color name = "colorAccent" >#2ECC71</color>
//</ resources >

// 22. Platforms/Android/MainActivity.cs
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

// 23. Converters/InvertedBoolConverter.cs
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

// 24. Converters/NullToBoolConverter.cs
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

// 25. Resources/Styles/Colors.xaml
//<? xml version="1.0" encoding="UTF-8" ?>
//<? xaml-comp compile = "true" ?>
//< ResourceDictionary
//    xmlns = "http://schemas.microsoft