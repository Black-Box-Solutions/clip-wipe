namespace ClipWipe.App.Services;

public interface IClipboardService : IDisposable
{
    event EventHandler<string>? ClipboardChanged;

    void StartListening();

    void StopListening();

    Task<string?> GetClipboardContentAsync();

    Task ClearClipboardAsync();

    Task<bool> HasClipboardContentAsync();

    DateTimeOffset? LastCleared { get; }
}
