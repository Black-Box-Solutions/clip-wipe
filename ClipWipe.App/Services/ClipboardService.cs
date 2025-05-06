namespace ClipWipe.App.Services;

/// <summary>
/// Platform-agnostic implementation for clipboard operations.
/// </summary>
public sealed partial class ClipboardService : IClipboardService
{
    // This partial class will be implemented differently for each platform
    // We'll define the common properties here
    public event EventHandler<string>? ClipboardChanged;

    public DateTimeOffset? LastCleared { get; private set; }

    // The platform-specific implementations will be in Platform folders

    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            // Release managed resources
            StopListening();
            DisposePlatformSpecificManaged(); // Call the platform-specific partial method
        }

        // Release unmanaged resources
        DisposePlatformSpecificUnmanaged();   // Call the platform-specific partial method

        _isDisposed = true;
    }

    partial void DisposePlatformSpecificManaged();

    partial void DisposePlatformSpecificUnmanaged();

    ~ClipboardService()
    {
        Dispose(false);
    }

    private void UpdateLastCleared()
    {
        LastCleared = DateTimeOffset.UtcNow;
    }
}
