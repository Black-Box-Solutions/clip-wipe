namespace ClipWipe.App.Services;

public interface IClipboardService
{
    Task<string?> GetClipboardContentAsync();

    Task ClearClipboardAsync();

    Task<bool> HasClipboardContentAsync();

    DateTime? LastCleared { get; }
}
