using Foundation;
using System.Diagnostics.CodeAnalysis;
using UIKit;

namespace ClipWipe.App.Services;

/// <summary>
/// Provides functionality for interacting with the system clipboard, including retrieving, clearing, and monitoring
/// clipboard content changes.
/// </summary>
/// <remarks>This service is designed for use in iOS applications and provides asynchronous methods for
/// clipboard operations. It also supports listening for clipboard content changes and  raising events when changes
/// occur.</remarks>
[SuppressMessage("Maintainability", "CA1513:Use ObjectDisposedException throw helper", Justification = "We need to use Interlocked")]
public partial class ClipboardService
{
    private NSObject? _observer;

    public void StartListening()
    {
        if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 1)
        {
            throw new ObjectDisposedException(nameof(ClipboardService));
        }

        _observer ??= NSNotificationCenter.DefaultCenter.AddObserver((NSString)"UIPasteboardChangedNotification", HandleClipboardChanged);
    }

    public void StopListening()
    {
        if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 1)
        {
            throw new ObjectDisposedException(nameof(ClipboardService));
        }

        if (_observer is not null)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(_observer);
            _observer.Dispose();
            _observer = null;
        }
    }

    private void HandleClipboardChanged(NSNotification notification)
    {
        //TODO is this thread-safe? is more synchronization needed here?
        string? content = UIPasteboard.General.String;

        // Use a local variable to ensure thread safety
        EventHandler<string>? handler = ClipboardChanged;
        handler?.Invoke(this, content ?? string.Empty);
    }

    public async Task<string?> GetClipboardContentAsync()
    {
        return await Task.Run(static () =>
        {
            UIPasteboard pasteboard = UIPasteboard.General;
            return pasteboard.HasStrings ? pasteboard.String : null;
        });
    }

    public async Task ClearClipboardAsync()
    {
        await Task.Run(() =>
        {
            UIPasteboard.General.String = string.Empty;
            UpdateLastCleared();
        });
    }

    public async Task<bool> HasClipboardContentAsync()
    {
        return await Task.Run(static () =>
        {
            UIPasteboard pasteboard = UIPasteboard.General;
            return pasteboard.HasStrings && !string.IsNullOrEmpty(pasteboard.String);
        });
    }
}