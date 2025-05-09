using Microsoft.Extensions.Logging;

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

    private int _isDisposed;
    private readonly ILogger<ClipboardService> _logger;

    public ClipboardService(ILogger<ClipboardService> logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 1)
        {
            return; // Already disposed
        }

        if (disposing)
        {
            // Release managed resources
            StopListening();
            DisposePlatformSpecificManaged(); // Call the platform-specific partial method
        }

        // Release unmanaged resources
        DisposePlatformSpecificUnmanaged();   // Call the platform-specific partial method
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
