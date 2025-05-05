namespace ClipWipe.App.Services;

public interface IClipboardService
{
    Task<string?> GetClipboardContentAsync();

    Task ClearClipboardAsync();

    Task<bool> HasClipboardContentAsync();

    DateTimeOffset? LastCleared { get; }
}
