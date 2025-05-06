using Microsoft.UI.Xaml;
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
public partial class ClipboardService
{
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
        catch
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

            throw;
        }
    }

    public void StopListening()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (_hwnd == default || _window is null)
        {
            throw new InvalidOperationException("Clipboard listener is not running.");
        }

        // Ensure that the window listener is properly stopped before releasing hwnd
        if (!PInvoke.RemoveClipboardFormatListener(_hwnd))
        {
            throw new InvalidOperationException("Failed to remove clipboard format listener.");
        }

        _window.Activated -= OnWindowActivated;
        _window = null;
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
                    OnClipboardUpdated();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while processing clipboard update messages in {nameof(OnWindowActivated)}.");
        }
    }

    private void HandleClipboardChanged()
    {
        //TODO: Uncomment and implement clipboard content retrieval for Windows
        //      string? content = UIPasteboard.General.String;
        //      ClipboardChanged?.Invoke(this, content ?? string.Empty);
        ClipboardChanged?.Invoke(this, string.Empty);
    }

    private static void OnClipboardUpdated()
    {
        // Handle clipboard content changes here
        Console.WriteLine("Clipboard content updated.");
    }

    partial void DisposePlatformSpecificUnmanaged()
    {
        // Windows-specific cleanup logic
        //TODO this is done in StopListening()
        //PInvoke.RemoveClipboardFormatListener(_hwnd);
        _hwnd = default;
    }
}
