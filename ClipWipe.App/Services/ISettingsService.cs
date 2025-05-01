namespace ClipWipe.App.Services;

public interface ISettingsService
{
    bool AutoClearEnabled { get; set; }

    int AutoClearIntervalMinutes { get; set; }

    bool StartOnBootEnabled { get; set; }

    DateTime? LastClearTime { get; set; }

    Task SaveSettingsAsync();

    Task LoadSettingsAsync();
}
