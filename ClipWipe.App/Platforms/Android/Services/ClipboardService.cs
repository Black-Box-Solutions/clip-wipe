using Android.Content;
using Android.OS;

namespace ClipWipe.App.Services;

/// <summary>
/// Provides functionality for interacting with the system clipboard, including reading, clearing,  and monitoring
/// clipboard content changes.
/// </summary>
/// <remarks>This service is designed for use in Android applications and provides asynchronous methods for
/// clipboard operations. It also supports listening for clipboard content changes and  raising events when changes
/// occur.</remarks>
public partial class ClipboardService
{
    private readonly Context _context = Android.App.Application.Context;
    private Android.Content.ClipboardManager? _clipboardManager;
    private ClipboardListener? _clipboardListener;

    public void StartListening()
    {
        _clipboardManager ??= (Android.Content.ClipboardManager?)_context.GetSystemService(Context.ClipboardService);
        if (_clipboardManager is not null)
        {
            _clipboardListener = new ClipboardListener(this);
            _clipboardManager.AddPrimaryClipChangedListener(_clipboardListener);
        }
    }

    public void StopListening()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (_clipboardManager is not null && _clipboardListener is not null)
        {
            // Unsubscribe from the listener
            _clipboardManager.RemovePrimaryClipChangedListener(_clipboardListener);
            _clipboardListener.Dispose();
            _clipboardListener = null;

            // Dispose of the clipboard manager
            _clipboardManager.Dispose();
            _clipboardManager = null;
        }
    }

    public async Task<string?> GetClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is Android.Content.ClipboardManager clipboardManager && clipboardManager.HasPrimaryClip)
            {
                ClipData? clipData = clipboardManager.PrimaryClip;
                if (clipData?.ItemCount > 0)
                {
                    return clipData.GetItemAt(0)?.Text;
                }
            }

            return null;
        });
    }

    public async Task ClearClipboardAsync()
    {
        await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is Android.Content.ClipboardManager clipboardManager)
            {
                if (Build.VERSION.SdkInt <= BuildVersionCodes.OMr1) // Check if API level is 27 or lower
                {
                    clipboardManager.PrimaryClip = ClipData.NewPlainText("", "");
                }
                else
                {
                    clipboardManager.ClearPrimaryClip(); // Clear the primary clipboard contents
                }

                UpdateLastCleared();
            }
        });
    }

    public async Task<bool> HasClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is
                Android.Content.ClipboardManager { HasPrimaryClip: true } clipboardManager)
            {
                ClipData? clipData = clipboardManager.PrimaryClip;
                if (clipData?.ItemCount > 0)
                {
                    return !string.IsNullOrEmpty(clipData.GetItemAt(0)?.Text);
                }
            }

            return false;
        });
    }

    private class ClipboardListener : Java.Lang.Object, Android.Content.ClipboardManager.IOnPrimaryClipChangedListener
    {
        private readonly ClipboardService _service;

        public ClipboardListener(ClipboardService service)
        {
            _service = service;
        }

        public void OnPrimaryClipChanged()
        {
            string? content = null;
            ClipData? clipData = _service._clipboardManager?.PrimaryClip;
            if (clipData?.ItemCount > 0)
            {
                content = clipData.GetItemAt(0)?.Text;
            }
            _service.ClipboardChanged?.Invoke(_service, content ?? string.Empty);
        }
    }
}
