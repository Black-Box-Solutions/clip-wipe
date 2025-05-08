using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System.Diagnostics.CodeAnalysis;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Window = Microsoft.UI.Xaml.Window;

namespace ClipWipe.App.Services;

/// <summary>
/// Provides functionality for interacting with the clipboard and listening for clipboard updates.
/// </summary>
/// <remarks>This service allows you to retrieve clipboard content, clear the clipboard, check for clipboard
/// content, and start or stop listening for clipboard updates. It is designed for use in Windows environments and
/// relies on platform-specific APIs for clipboard operations and event handling.</remarks>
[SuppressMessage("Maintainability", "CA1513:Use ObjectDisposedException throw helper", Justification = "We need to use Interlocked")]
public partial class ClipboardService
{
    private static readonly Action<ILogger, string, Exception?> _logClipboardUpdated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(OnClipboardUpdatedAsync)),
            "Clipboard updated: {Content}");

    private static readonly Action<ILogger, string, Exception?> _logErrorProcessingClipboardUpdate =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2, nameof(OnWindowActivated)),
            "Error while processing clipboard update messages in {MethodName}.");

    private static readonly Action<ILogger, string, Exception?> _logErrorStartingClipboardListener =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3, nameof(StartListening)),
            "Error while starting clipboard listener in {MethodName}.");

    private static readonly Action<ILogger, string, Exception?> _logErrorStoppingClipboardListener =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(4, nameof(StopListening)),
            "Error while stopping clipboard listener in {MethodName}.");

    private readonly Lock _lock = new Lock();
    private Window? _window;
    private HWND _hwnd;

    public async Task<string?> GetClipboardContentAsync()
    {
        return Clipboard.HasText ? await Clipboard.GetTextAsync() : null;
    }

    public async Task ClearClipboardAsync()
    {
        await Clipboard.SetTextAsync(string.Empty);
        UpdateLastCleared();
    }

    public Task<bool> HasClipboardContentAsync()
    {
        return Task.FromResult(Clipboard.HasText);
    }

    public void StartListening()
    {
        lock (_lock)
        {
            if (_window is not null)
            {
                throw new InvalidOperationException("Clipboard listener is already running.");
            }

            try
            {
                _window = new Window();
                _window.Activated += OnWindowActivated;

                nint hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_window);
                _hwnd = new HWND(hwnd);

                if (!PInvoke.AddClipboardFormatListener(_hwnd))
                {
                    throw new InvalidOperationException("Failed to add clipboard format listener.");
                }
            }
            catch (Exception ex)
            {
                _logErrorStartingClipboardListener(_logger, nameof(StartListening), ex);
                Cleanup();
                throw;
            }
        }
    }

    public void StopListening()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        lock (_lock)
        {
            if (_hwnd == default || _window is null)
            {
                throw new InvalidOperationException("Clipboard listener is not running.");
            }

            try
            {
                if (!PInvoke.RemoveClipboardFormatListener(_hwnd))
                {
                    throw new InvalidOperationException("Failed to remove clipboard format listener.");
                }

                _window.Activated -= OnWindowActivated;
                _window = null;
            }
            catch (Exception ex)
            {
                _logErrorStoppingClipboardListener(_logger, nameof(StopListening), ex);
                throw;
            }
            finally
            {
                Cleanup();
            }
        }
    }

    private void OnWindowActivated(object sender, WindowActivatedEventArgs e)
    {
        if (e.WindowActivationState == WindowActivationState.Deactivated)
        {
            return;
        }

        try
        {
            // Handle clipboard update messages
            while (PInvoke.PeekMessage(out MSG message, _hwnd, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE))
            {
                if (message.message == PInvoke.WM_CLIPBOARDUPDATE)
                {
                    _ = OnClipboardUpdatedAsync(); // Fire-and-forget, but safer with Task
                }
            }
        }
        catch (Exception ex)
        {
            _logErrorProcessingClipboardUpdate(_logger, nameof(OnWindowActivated), ex);
        }
    }

    //TODO verify this is correct
    private async Task OnClipboardUpdatedAsync()
    {
        string? content = await GetClipboardContentAsync();

        // Use a local variable to ensure thread safety
        EventHandler<string>? handler = ClipboardChanged;
        handler?.Invoke(this, content ?? string.Empty);
        _logClipboardUpdated(_logger, content ?? string.Empty, null);
    }

    //TODO this should probably be in the Dispose method
    private void Cleanup()
    {
        lock (_lock)
        {
            if (_window is not null)
            {
                _window.Activated -= OnWindowActivated;
                _window = null;
            }
            if (_hwnd != default)
            {
                PInvoke.RemoveClipboardFormatListener(_hwnd);
                _hwnd = default;
            }
        }
    }

    partial void DisposePlatformSpecificUnmanaged()
    {
        // Windows-specific cleanup logic
        lock (_lock)
        {
            //TODO this is done in StopListening()
            if (_hwnd != default)
            {
                //PInvoke.RemoveClipboardFormatListener(_hwnd);
                _hwnd = default;
            }
        }
    }
}
