// This is a .NET MAUI application for managing clipboard content on Android
// Project structure will include:
// 1. Main app files
// 2. Platform-specific implementations
// 3. Services for clipboard management
// 4. UI for the application

// Here's the solution structure I'll implement:

// ---------------------------------------
// 1. MAIN PROJECT FILES
// ---------------------------------------

// App.xaml
/*
*/

// App.xaml.cs
/*
using ClipboardManager.Services;

namespace ClipboardManager;

*/

// AppShell.xaml
/*
*/

// AppShell.xaml.cs
/*
*/

// MauiProgram.cs
/*
using ClipboardManager.Services;
using ClipboardManager.ViewModels;
using ClipboardManager.Views;
using Microsoft.Extensions.Logging;

namespace ClipboardManager;

*/

// ---------------------------------------
// 2. SERVICES
// ---------------------------------------

// Services/IClipboardService.cs
/*
namespace ClipboardManager.Services;

*/

// Services/ClipboardService.cs (Platform-independent implementation)
/*
namespace ClipboardManager.Services;

*/

// Services/ISettingsService.cs
/*
namespace ClipboardManager.Services;

*/

// Services/SettingsService.cs
/*
namespace ClipboardManager.Services;

*/

// Services/ClipboardTimerService.cs
/*
namespace ClipboardManager.Services;

*/

// ---------------------------------------
// 3. VIEWMODELS
// ---------------------------------------

// ViewModels/BaseViewModel.cs
/*
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClipboardManager.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, value))
        {
            return false;
        }
        
        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
*/

// ViewModels/MainPageViewModel.cs
/*
using System.Windows.Input;
using ClipboardManager.Services;

namespace ClipboardManager.ViewModels;

public class MainPageViewModel : BaseViewModel
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
    
    public ICommand RefreshClipboardCommand { get; }
    public ICommand ClearClipboardCommand { get; }
    
    public MainPageViewModel(
        IClipboardService clipboardService,
        ISettingsService settingsService,
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
            StatusMessage = $"Error clearing clipboard: {ex.Message}";
        }
    }
}
*/

// ViewModels/SettingsPageViewModel.cs
/*
using System.Windows.Input;
using ClipboardManager.Services;

namespace ClipboardManager.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly ClipboardTimerService _clipboardTimerService;
    
    private bool _autoClearEnabled;
    private int _autoClearIntervalMinutes;
    private bool _startOnBootEnabled;
    private string _statusMessage;
    
    public bool AutoClearEnabled
    {
        get => _autoClearEnabled;
        set
        {
            if (SetProperty(ref _autoClearEnabled, value))
            {
                _settingsService.AutoClearEnabled = value;
                UpdateTimerService();
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
                UpdateTimerService();
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
    
    public ICommand SaveSettingsCommand { get; }
    
    public SettingsPageViewModel(
        ISettingsService settingsService,
        ClipboardTimerService clipboardTimerService)
    {
        _settingsService = settingsService;
        _clipboardTimerService = clipboardTimerService;
        
        SaveSettingsCommand = new Command(SaveSettings);
        
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        _autoClearEnabled = _settingsService.AutoClearEnabled;
        _autoClearIntervalMinutes = _settingsService.AutoClearIntervalMinutes;
        _startOnBootEnabled = _settingsService.StartOnBootEnabled;
    }
    
    private void SaveSettings()
    {
        try
        {
            _settingsService.SaveSettings();
            StatusMessage = "Settings saved successfully.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving settings: {ex.Message}";
        }
    }
    
    private void UpdateTimerService()
    {
        if (_autoClearEnabled)
        {
            _clipboardTimerService.StartTimer(_autoClearIntervalMinutes);
            StatusMessage = $"Auto-clear timer set for {_autoClearIntervalMinutes} minutes.";
        }
        else
        {
            _clipboardTimerService.StopTimer();
            StatusMessage = "Auto-clear timer disabled.";
        }
    }
}
*/

// ---------------------------------------
// 4. VIEWS
// ---------------------------------------

// Views/MainPage.xaml
/*
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewmodels="clr-namespace:ClipboardManager.ViewModels"
             x:Class="ClipboardManager.Views.MainPage"
             x:DataType="viewmodels:MainPageViewModel"
             Title="Clipboard Manager">

    <ScrollView>
        <VerticalStackLayout Padding="25" Spacing="20">
            <Label Text="Clipboard Manager" FontSize="24" FontAttributes="Bold" HorizontalOptions="Center" />
            
            <Frame BorderColor="LightGray" Padding="15" HasShadow="True">
                <VerticalStackLayout Spacing="10">
                    <Label Text="Current Clipboard Content" FontSize="16" FontAttributes="Bold" />
                    
                    <Border StrokeShape="RoundRectangle 10,10,10,10"
                            Stroke="LightGray"
                            StrokeThickness="1"
                            Padding="10">
                        <ScrollView MaximumHeightRequest="150">
                            <Label Text="{Binding ClipboardContent}" 
                                   IsVisible="{Binding HasClipboardContent}"
                                   LineBreakMode="WordWrap"/>
                        </ScrollView>
                    </Border>
                    
                    <Label Text="Clipboard is empty" 
                           IsVisible="{Binding HasClipboardContent, Converter={StaticResource InvertedBoolConverter}}"
                           HorizontalOptions="Center"
                           TextColor="Gray"/>
                    
                    <Button Text="Refresh Clipboard"
                            Command="{Binding RefreshClipboardCommand}"
                            BackgroundColor="#3498db"
                            TextColor="White"
                            Margin="0,10,0,0"/>
                </VerticalStackLayout>
            </Frame>
            
            <Frame BorderColor="LightGray" Padding="15" HasShadow="True">
                <VerticalStackLayout Spacing="10">
                    <Label Text="Clipboard Actions" FontSize="16" FontAttributes="Bold" />
                    
                    <Button Text="Clear Clipboard"
                            Command="{Binding ClearClipboardCommand}"
                            BackgroundColor="#e74c3c"
                            TextColor="White"/>
                    
                    <VerticalStackLayout IsVisible="{Binding LastClearedTime, Converter={StaticResource NullToBoolConverter}}">
                        <Label Text="Last Cleared:" FontAttributes="Bold" />
                        <Label Text="{Binding LastClearedTime, StringFormat='{0:MM/dd/yyyy hh:mm tt}'}" />
                    </VerticalStackLayout>
                    
                    <Label Text="{Binding StatusMessage}" 
                           TextColor="DimGray"
                           HorizontalOptions="Center"
                           Margin="0,10,0,0"/>
                </VerticalStackLayout>
            </Frame>
            
            <Label Text="Press 'Refresh Clipboard' to check current clipboard content. Press 'Clear Clipboard' to clear clipboard content immediately."
                   TextColor="Gray"
                   HorizontalOptions="Center"
                   HorizontalTextAlignment="Center"
                   Margin="0,10,0,0"/>

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
*/

// Views/MainPage.xaml.cs
/*
using ClipboardManager.ViewModels;

namespace ClipboardManager.Views;

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
        _viewModel.RefreshClipboardCommand.Execute(null);
    }
}
*/

// Views/SettingsPage.xaml
/*
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewmodels="clr-namespace:ClipboardManager.ViewModels"
             x:Class="ClipboardManager.Views.SettingsPage"
             x:DataType="viewmodels:SettingsPageViewModel"
             Title="Settings">

    <ScrollView>
        <VerticalStackLayout Padding="25" Spacing="20">
            <Label Text="Clipboard Manager Settings" FontSize="24" FontAttributes="Bold" HorizontalOptions="Center" />
            
            <Frame BorderColor="LightGray" Padding="15" HasShadow="True">
                <VerticalStackLayout Spacing="15">
                    <Label Text="Auto-Clear Settings" FontSize="16" FontAttributes="Bold" />
                    
                    <HorizontalStackLayout Spacing="10">
                        <CheckBox IsChecked="{Binding AutoClearEnabled}" />
                        <Label Text="Enable Auto-Clear Clipboard" VerticalOptions="Center" />
                    </HorizontalStackLayout>
                    
                    <VerticalStackLayout IsVisible="{Binding AutoClearEnabled}">
                        <Label Text="Clear Interval (in minutes):" Margin="0,0,0,5" />
                        <HorizontalStackLayout Spacing="15">
                            <Slider Value="{Binding AutoClearIntervalMinutes}"
                                    Minimum="1"
                                    Maximum="120"
                                    MinimumTrackColor="#3498db"
                                    MaximumTrackColor="LightGray"
                                    ThumbColor="#3498db"
                                    HorizontalOptions="FillAndExpand"/>
                            <Label Text="{Binding AutoClearIntervalMinutes, StringFormat='{0} min'}"
                                   VerticalOptions="Center"
                                   WidthRequest="50"/>
                        </HorizontalStackLayout>
                    </VerticalStackLayout>
                </VerticalStackLayout>
            </Frame>
            
            <Frame BorderColor="LightGray" Padding="15" HasShadow="True">
                <VerticalStackLayout Spacing="15">
                    <Label Text="System Settings" FontSize="16" FontAttributes="Bold" />
                    
                    <HorizontalStackLayout Spacing="10">
                        <CheckBox IsChecked="{Binding StartOnBootEnabled}" />
                        <Label Text="Start on device boot" VerticalOptions="Center" />
                    </HorizontalStackLayout>
                    
                    <Button Text="Save Settings"
                            Command="{Binding SaveSettingsCommand}"
                            BackgroundColor="#2ecc71"
                            TextColor="White"
                            Margin="0,10,0,0"/>
                    
                    <Label Text="{Binding StatusMessage}" 
                           TextColor="DimGray"
                           HorizontalOptions="Center"/>
                </VerticalStackLayout>
            </Frame>
            
            <Label Text="Auto-clear will automatically clear your clipboard after the specified interval. Start on device boot will launch the app automatically when your device starts."
                   TextColor="Gray"
                   HorizontalOptions="Center"
                   HorizontalTextAlignment="Center"
                   Margin="0,10,0,0"/>

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
*/

// Views/SettingsPage.xaml.cs
/*
using ClipboardManager.ViewModels;

namespace ClipboardManager.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsPageViewModel _viewModel;
    
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}
*/

// ---------------------------------------
// 5. PLATFORM-SPECIFIC CODE (ANDROID)
// ---------------------------------------

// Platforms/Android/ClipboardService.cs
/*
using Android.Content;
using ClipboardManager = Android.Content.ClipboardManager;

*/

// Platforms/Android/AndroidManifest.xml
/*
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    <application android:allowBackup="true" android:icon="@mipmap/appicon" android:roundIcon="@mipmap/appicon_round" android:supportsRtl="true"></application>
    <uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED" />
    <uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
</manifest>
*/

// Platforms/Android/BootReceiver.cs
/*
using Android.App;
using Android.Content;
using ClipboardManager.Services;

namespace ClipboardManager.Platforms.Android;

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent.Action == Intent.ActionBootCompleted)
        {
            // Check if start on boot is enabled in settings
            var preferences = context.GetSharedPreferences("ClipboardManager.Settings", FileCreationMode.Private);
            bool startOnBootEnabled = preferences.GetBoolean("start_on_boot_enabled", false);
            
            if (startOnBootEnabled)
            {
                var startIntent = new Intent(context, typeof(MainActivity));
                startIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(startIntent);
            }
        }
    }
}
*/

// Platforms/Android/Resources/values/colors.xml
/*
<?xml version="1.0" encoding="utf-8"?>
<resources>
    <color name="colorPrimary">#3498DB</color>
    <color name="colorPrimaryDark">#2980B9</color>
    <color name="colorAccent">#2ECC71</color>
</resources>
*/

// Platforms/Android/MainActivity.cs
/*
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ClipboardManager;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}
*/

// ---------------------------------------
// 6. RESOURCES AND CONVERTERS
// ---------------------------------------

// Converters/InvertedBoolConverter.cs
/*
using System.Globalization;

namespace ClipboardManager.Converters;

public class InvertedBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
                    if (value is bool boolValue)
        {
            return !boolValue;
        }
        
        return true;
    }
}
*/

// Converters/NullToBoolConverter.cs
/*
using System.Globalization;

namespace ClipboardManager.Converters;

public class NullToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null;
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
*/

// Resources/Styles/Colors.xaml
/*
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <Color x:Key="Primary">#3498db</Color>
    <Color x:Key="Secondary">#2ecc71</Color>
    <Color x:Key="Tertiary">#e74c3c</Color>
    <Color x:Key="White">White</Color>
    <Color x:Key="Black">Black</Color>
    <Color x:Key="Gray100">#E1E1E1</Color>
    <Color x:Key="Gray200">#C8C8C8</Color>
    <Color x:Key="Gray300">#ACACAC</Color>
    <Color x:Key="Gray400">#919191</Color>
    <Color x:Key="Gray500">#6E6E6E</Color>
    <Color x:Key="Gray600">#404040</Color>
    <Color x:Key="Gray900">#212121</Color>
    <Color x:Key="Gray950">#141414</Color>
    <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource Primary}"/>
    <SolidColorBrush x:Key="SecondaryBrush" Color="{StaticResource Secondary}"/>
    <SolidColorBrush x:Key="TertiaryBrush" Color="{StaticResource Tertiary}"/>
    <SolidColorBrush x:Key="WhiteBrush" Color="{StaticResource White}"/>
    <SolidColorBrush x:Key="BlackBrush" Color="{StaticResource Black}"/>
    <SolidColorBrush x:Key="Gray100Brush" Color="{StaticResource Gray100}"/>
    <SolidColorBrush x:Key="Gray200Brush" Color="{StaticResource Gray200}"/>
    <SolidColorBrush x:Key="Gray300Brush" Color="{StaticResource Gray300}"/>
    <SolidColorBrush x:Key="Gray400Brush" Color="{StaticResource Gray400}"/>
    <SolidColorBrush x:Key="Gray500Brush" Color="{StaticResource Gray500}"/>
    <SolidColorBrush x:Key="Gray600Brush" Color="{StaticResource Gray600}"/>
    <SolidColorBrush x:Key="Gray900Brush" Color="{StaticResource Gray900}"/>
    <SolidColorBrush x:Key="Gray950Brush" Color="{StaticResource Gray950}"/>

    <Color x:Key="Yellow100Accent">#F7B548</Color>
    <Color x:Key="Yellow200Accent">#FFD590</Color>
    <Color x:Key="Yellow300Accent">#FFE5B9</Color>
    <Color x:Key="Cyan100Accent">#28C2D1</Color>
    <Color x:Key="Cyan200Accent">#7BDDEF</Color>
    <Color x:Key="Cyan300Accent">#C3F2F4</Color>
    <Color x:Key="Blue100Accent">#3E8EED</Color>
    <Color x:Key="Blue200Accent">#72ACF1</Color>
    <Color x:Key="Blue300Accent">#A7CBF6</Color>

</ResourceDictionary>
*/

// Resources/Styles/Styles.xaml
/*
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:converters="clr-namespace:ClipboardManager.Converters">

    <Style TargetType="ActivityIndicator">
        <Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource White}}" />
    </Style>

    <Style TargetType="IndicatorView">
        <Setter Property="IndicatorColor" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray500}}" />
        <Setter Property="SelectedIndicatorColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource White}}" />
    </Style>

    <Style TargetType="Border">
        <Setter Property="Stroke" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray500}}" />
        <Setter Property="StrokeShape" Value="Rectangle" />
        <Setter Property="StrokeThickness" Value="1" />
    </Style>

    <Style TargetType="BoxView">
        <Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Gray950}, Dark={StaticResource Gray200}}" />
    </Style>

    <Style TargetType="Button">
        <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource White}, Dark={StaticResource White}}" />
        <Setter Property="BackgroundColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
        <Setter Property="FontFamily" Value="OpenSansRegular" />
        <Setter Property="FontSize" Value="14" />
        <Setter Property="CornerRadius" Value="8" />
        <Setter Property="Padding" Value="14,10" />
    </Style>

    <Style TargetType="CheckBox">
        <Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
        <Setter Property="VisualStateManager.VisualStateGroups">
            <VisualStateGroupList>
                <VisualStateGroup x:Name="CommonStates">
                    <VisualState x:Name="Normal" />
                    <VisualState x:Name="Disabled">
                        <VisualState.Setters>
                            <Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Gray300}, Dark={StaticResource Gray600}}" />
                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateGroupList>
        </Setter>
    </Style>

    <Style TargetType="Frame">
        <Setter Property="HasShadow" Value="False" />
        <Setter Property="BorderColor" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray500}}" />
        <Setter Property="CornerRadius" Value="10" />
    </Style>

    <Style TargetType="Label">
        <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource Black}, Dark={StaticResource White}}" />
        <Setter Property="FontFamily" Value="OpenSansRegular" />
        <Setter Property="FontSize" Value="14" />
        <Setter Property="VisualStateManager.VisualStateGroups">
            <VisualStateGroupList>
                <VisualStateGroup x:Name="CommonStates">
                    <VisualState x:Name="Normal" />
                    <VisualState x:Name="Disabled">
                        <VisualState.Setters>
                            <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource Gray300}, Dark={StaticResource Gray600}}" />
                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateGroupList>
        </Setter>
    </Style>

    <Style TargetType="Slider">
        <Setter Property="MinimumTrackColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
        <Setter Property="MaximumTrackColor" Value="{AppThemeBinding Light={StaticResource Gray200}, Dark={StaticResource Gray600}}" />
        <Setter Property="ThumbColor" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource Primary}}" />
    </Style>

    <converters:InvertedBoolConverter x:Key="InvertedBoolConverter"/>
    <converters:NullToBoolConverter x:Key="NullToBoolConverter"/>

</ResourceDictionary>
*/

// ---------------------------------------
// 7. ADDITIONAL FILES
// ---------------------------------------

// ClipboardManager.csproj
/*
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFrameworks>net9.0-android</TargetFrameworks>
        <OutputType>Exe</OutputType>
        <RootNamespace>ClipboardManager</RootNamespace>
        <UseMaui>true</UseMaui>
        <SingleProject>true</SingleProject>
        <ImplicitUsings>enable</ImplicitUsings>
        
        <!-- Display name -->
        <ApplicationTitle>Clipboard Manager</ApplicationTitle>
        
        <!-- App Identifier -->
        <ApplicationId>com.yourcompany.clipboardmanager</ApplicationId>
        <ApplicationIdGuid>56CD2C99-8F78-4AEE-87E5-999A28C9EA92</ApplicationIdGuid>
        
        <!-- Versions -->
        <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
        <ApplicationVersion>1</ApplicationVersion>
        
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
    </PropertyGroup>

    <ItemGroup>
        <!-- App Icon -->
        <MauiIcon Include="Resources\AppIcon\appicon.svg" ForegroundFile="Resources\AppIcon\appiconfg.svg" Color="#3498DB" />
        
        <!-- Splash Screen -->
        <MauiSplashScreen Include="Resources\Splash\splash.svg" Color="#3498DB" BaseSize="128,128" />
        
        <!-- Images -->
        <MauiImage Include="Resources\Images\*" />
        
        <!-- Custom Fonts -->
        <MauiFont Include="Resources\Fonts\*" />
        
        <!-- Raw Assets (also remove the "Resources\Raw" prefix) -->
        <MauiAsset Include="Resources\Raw\**" LogicalName="%(RecursiveDir)%(Filename)%(Extension)" />
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
        <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
        <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.0-preview.1.24080.9" />
    </ItemGroup>

</Project>
*/

// README.md
/*
# Clipboard Manager for Android

A .NET MAUI application for managing clipboard content on Android devices.

## Features

- View current clipboard content
- Clear clipboard content manually with one tap
- Auto-clear clipboard based on a configurable timer
- Start application automatically on device boot (optional)
- Simple and intuitive user interface

## Requirements

- Android 5.0 (API level 21) or higher
- Samsung devices running One UI (tested on Galaxy S25+)

## Building and Running

This application is built using .NET MAUI with .NET 9.

1. Clone the repository
2. Open the solution in Visual Studio 2022 or later
3. Build and run the application on an Android device or emulator

## Implementation Details

The application is structured using the MVVM (Model-View-ViewModel) pattern:

- **Views**: UI components (MainPage, SettingsPage)
- **ViewModels**: Business logic and UI state (MainPageViewModel, SettingsPageViewModel)
- **Services**: Core functionality (ClipboardService, SettingsService, ClipboardTimerService)

The application uses platform-specific code to interact with the Android clipboard API.

## Samsung OneUI Compatibility

The application has been tested on Samsung devices running OneUI. While Samsung does implement some clipboard access alerts, this application is designed to work with these constraints.

## License

[MIT License](LICENSE)
*/

// ---------------------------------------
// 8. RESOURCES (Additional files needed)
// ---------------------------------------

// Resources/Images/clipboard.png - Add a clipboard icon image
// Resources/Images/settings.png - Add a settings icon image

// Resources/AppIcon/appicon.svg
/*
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
  <rect width="512" height="512" rx="50" ry="50" fill="#3498db"/>
  <rect x="120" y="80" width="272" height="352" rx="20" ry="20" fill="#ffffff"/>
  <rect x="160" y="120" width="192" height="30" rx="5" ry="5" fill="#e74c3c"/>
  <rect x="160" y="170" width="192" height="30" rx="5" ry="5" fill="#f1c40f"/>
  <rect x="160" y="220" width="192" height="30" rx="5" ry="5" fill="#2ecc71"/>
  <rect x="160" y="270" width="192" height="30" rx="5" ry="5" fill="#9b59b6"/>
  <rect x="160" y="320" width="120" height="30" rx="5" ry="5" fill="#3498db"/>
</svg>
*/

// Resources/AppIcon/appiconfg.svg
/*
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
  <rect x="120" y="80" width="272" height="352" rx="20" ry="20" fill="#ffffff"/>
  <rect x="160" y="120" width="192" height="30" rx="5" ry="5" fill="#e74c3c"/>
  <rect x="160" y="170" width="192" height="30" rx="5" ry="5" fill="#f1c40f"/>
  <rect x="160" y="220" width="192" height="30" rx="5" ry="5" fill="#2ecc71"/>
  <rect x="160" y="270" width="192" height="30" rx="5" ry="5" fill="#9b59b6"/>
  <rect x="160" y="320" width="120" height="30" rx="5" ry="5" fill="#3498db"/>
</svg>
*/

// Resources/Splash/splash.svg
/*
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
  <rect x="120" y="80" width="272" height="352" rx="20" ry="20" fill="#ffffff"/>
  <rect x="160" y="120" width="192" height="30" rx="5" ry="5" fill="#e74c3c"/>
  <rect x="160" y="170" width="192" height="30" rx="5" ry="5" fill="#f1c40f"/>
  <rect x="160" y="220" width="192" height="30" rx="5" ry="5" fill="#2ecc71"/>
  <rect x="160" y="270" width="192" height="30" rx="5" ry="5" fill="#9b59b6"/>
  <rect x="160" y="320" width="120" height="30" rx="5" ry="5" fill="#3498db"/>
</svg>
*/

// ---------------------------------------
// 9. FINAL NOTES AND CONFIGURATION
// ---------------------------------------

/*
//TODO Additional Steps to Complete the Project:

1. Configure Visual Studio:
   - Use Visual Studio 2022 or later with .NET MAUI workload installed
   - Ensure you have the Android SDK installed for API 21 or higher
   - Add Android SDK platform tools to your PATH environment variable

2. Configure the Android project:
   - Update the application ID in ClipboardManager.csproj to your preferred package name
   - Generate a signing key for production release

3. Test on multiple Android versions:
   - Test on Android 5.0 (API 21) for minimum compatibility
   - Test specifically on Samsung devices running OneUI for proper clipboard integration
   - Verify boot receiver functionality

4. Optional Improvements:
   - Add notification to indicate when clipboard was automatically cleared
   - Implement clipboard history (if desired, with proper security)
   - Add biometric authentication for viewing clipboard contents
   - Implement dark mode support
   - Add widget for quick clipboard clearing from home screen
*/

//        return !boolValue;
//    }

//    return false;
//}

//public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//{
//    if (value is bool boolValue)
//    {