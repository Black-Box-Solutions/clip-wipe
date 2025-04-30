using Android.App;
using Android.Content.PM;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace ClipWipe.App;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}