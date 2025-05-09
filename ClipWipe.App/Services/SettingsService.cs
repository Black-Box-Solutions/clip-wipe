using ClipWipe.App.Extensions;

namespace ClipWipe.App.Services;

public class SettingsService : ISettingsService
{
    private const string AutoClearEnabledKey = "auto_clear_enabled";
    private const string AutoClearIntervalKey = "auto_clear_interval_minutes";
    private const string StartOnBootEnabledKey = "start_on_boot_enabled";
    private const string LastClearTimeKey = "last_clear_time";

    private bool _autoClearEnabled;
    private int _autoClearIntervalMinutes;
    private bool _startOnBootEnabled;
    private DateTimeOffset? _lastClearTime;
    private const int DefaultAutoClearIntervalMinutes = 60;

    public SettingsService()
    {
        //TODO LoadSettingsAsync().ConfigureAwait(false);
        LoadSettingsAsync().SafeFireAndForget();
    }

    public bool AutoClearEnabled
    {
        get => _autoClearEnabled;
        set
        {
            _autoClearEnabled = value;
            Preferences.Default.Set(AutoClearEnabledKey, value);
        }
    }

    public int AutoClearIntervalMinutes
    {
        get => _autoClearIntervalMinutes;
        set
        {
            _autoClearIntervalMinutes = value;
            Preferences.Default.Set(AutoClearIntervalKey, value);
        }
    }

    public bool StartOnBootEnabled
    {
        get => _startOnBootEnabled;
        set
        {
            _startOnBootEnabled = value;
            Preferences.Default.Set(StartOnBootEnabledKey, value);
        }
    }

    public DateTimeOffset? LastClearTime
    {
        get => _lastClearTime;
        set
        {
            _lastClearTime = value;
            if (value.HasValue)
            {
                Preferences.Default.Set(LastClearTimeKey, value.Value.ToString("o"));
            }
            else
            {
                Preferences.Default.Remove(LastClearTimeKey);
            }
        }
    }

    public async Task SaveSettingsAsync()
    {
        await Task.Run(() =>
        {
            Preferences.Default.Set(AutoClearEnabledKey, _autoClearEnabled);
            Preferences.Default.Set(AutoClearIntervalKey, _autoClearIntervalMinutes);
            Preferences.Default.Set(StartOnBootEnabledKey, _startOnBootEnabled);

            if (_lastClearTime.HasValue)
            {
                Preferences.Default.Set(LastClearTimeKey, _lastClearTime.Value.ToString("o"));
            }
        });
    }

    public async Task LoadSettingsAsync()
    {
        await Task.Run(() =>
        {
            _autoClearEnabled = Preferences.Default.Get(AutoClearEnabledKey, false);
            _autoClearIntervalMinutes = Preferences.Default.Get(AutoClearIntervalKey, DefaultAutoClearIntervalMinutes);
            _startOnBootEnabled = Preferences.Default.Get(StartOnBootEnabledKey, false);

            string lastClearTimeStr = Preferences.Default.Get(LastClearTimeKey, string.Empty);
            if (!string.IsNullOrEmpty(lastClearTimeStr) && DateTimeOffset.TryParse(lastClearTimeStr, out DateTimeOffset lastClearTime))
            {
                _lastClearTime = lastClearTime;
            }
            else
            {
                _lastClearTime = null;
            }
        });
    }
}
