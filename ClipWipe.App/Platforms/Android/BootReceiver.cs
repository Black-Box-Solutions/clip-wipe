using Android.App;
using Android.Content;

namespace ClipWipe.App.Platforms.Android;

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent.Action == Intent.ActionBootCompleted)
        {
            // Check if start on boot is enabled in settings
            var preferences = context.GetSharedPreferences("ClipWipe.App.Settings", FileCreationMode.Private);
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
