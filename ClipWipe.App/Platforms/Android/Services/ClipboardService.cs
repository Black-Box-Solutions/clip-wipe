using Android.Content;
using Android.OS;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ClipWipe.App.Services;

/// <summary>
/// Provides functionality for interacting with the system clipboard, including reading, clearing,  and monitoring
/// clipboard content changes.
/// </summary>
/// <remarks>This service is designed for use in Android applications and provides asynchronous methods for
/// clipboard operations. It also supports listening for clipboard content changes and  raising events when changes
/// occur.</remarks>
[SuppressMessage("Maintainability", "CA1513:Use ObjectDisposedException throw helper", Justification = "We need to use Interlocked")]
public partial class ClipboardService
{
    private readonly Context _context = Android.App.Application.Context;
    private Android.Content.ClipboardManager? _clipboardManager;
    private ClipboardListener? _clipboardListener;

    public void StartListening()
    {
        //if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 1)
        //{
        //    throw new ObjectDisposedException(nameof(ClipboardService));
        //}

        //if (_clipboardManager is null)
        //{
        //    _clipboardManager = (Android.Content.ClipboardManager?)_context.GetSystemService(Context.ClipboardService);
        //}

        //if (_clipboardManager is not null)
        //{
        //    _clipboardListener = new ClipboardListener(this);
        //    _clipboardManager.AddPrimaryClipChangedListener(_clipboardListener);
        //}
    }

    public void StopListening()
    {
        //if (Interlocked.CompareExchange(ref _isDisposed, 0, 0) == 1)
        //{
        //    throw new ObjectDisposedException(nameof(ClipboardService));
        //}

        //if (_clipboardManager is not null && _clipboardListener is not null)
        //{
        //    // Unsubscribe from the listener
        //    _clipboardManager.RemovePrimaryClipChangedListener(_clipboardListener);
        //    _clipboardListener.Dispose();
        //    _clipboardListener = null;

        //    // Dispose of the clipboard manager
        //    _clipboardManager.Dispose();
        //    _clipboardManager = null;
        //}
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
        await Task.Run(async () =>
        {
            // Android's implementation varies by manufacturer, but we'll just clear the current clipboard
            if (_context.GetSystemService(Context.ClipboardService) is Android.Content.ClipboardManager clipboardManager)
            {
                try
                {
                    //List<string?> clipItems = await QuerySamsungClipboardContentsAsync();
                    //foreach (string? item in clipItems)
                    //{
                    //    _logger.LogInformation("Clipboard item: {Item}", item);
                    //    System.Diagnostics.Debug.WriteLine($"Clipboard item: {item}");
                    //}

                    // Try to access Samsung's clipboard content provider
                    // URI may vary based on Samsung's implementation
                    Android.Net.Uri? clipboardUri = Android.Net.Uri.Parse("content://com.samsung.android.content.clipboard/clip");
                    if (clipboardUri is not null && _context.ContentResolver != null)
                    {
                        using ContentProviderClient? client = _context.ContentResolver.AcquireContentProviderClient(clipboardUri);
                        int? rowsDeleted = client?.Delete(clipboardUri, null, null);
                        _logger.LogInformation("Clipboard cleared using content provider. Rows deleted: {RowsDeleted}", rowsDeleted);

                        // This may not be necessary, but it's a good practice to clear the clipboard after deleting the content
                        clipboardManager.PrimaryClip = ClipData.NewPlainText("empty", "");
                        clipboardManager.ClearPrimaryClip(); // Clear the primary clipboard contents

                        bool? refreshed = client?.Refresh(clipboardUri, null, null);
                        if (refreshed == true)
                        {
                            // Successfully refreshed the clipboard
                            _logger.LogInformation("Clipboard cleared using content provider.");
                        }
                        else
                        {
                            // Failed to refresh the clipboard, but we can still clear it
                            _logger.LogWarning("Failed to refresh clipboard after clearing using content provider.");
                        }

                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Content provider-based clipboard clear failed");
                }

                try
                {
                    if (Build.Manufacturer?.Contains("samsung", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        // Samsung workaround - use repeated different empty text
                        // This helps displace items in Samsung's clipboard history
                        for (int i = 0; i < 50; i++)
                        {
                            string placeholder = new string(' ', i + 1); // Different length spaces
                            clipboardManager.PrimaryClip = ClipData.NewPlainText($"empty_{i}", placeholder);

                            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
                            {
                                clipboardManager.ClearPrimaryClip();
                            }

                            // Small delay between operations
                            await Task.Delay(50);
                        }

                        // Final clear with empty string
                        clipboardManager.PrimaryClip = ClipData.NewPlainText("empty", "");

                        if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
                        {
                            clipboardManager.ClearPrimaryClip();
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "special loop-based clipboard clear failed");
                }
                //for (int i = 0; i < 10; i++)
                //{
                //    clipboardManager.Text = string.Empty;
                //    clipboardManager.PrimaryClip = ClipData.NewPlainText("", "");
                //    clipboardManager.ClearPrimaryClip(); // Clear the primary clipboard contents
                //}

                //if (Build.VERSION.SdkInt <= BuildVersionCodes.OMr1) // Check if API level is 27 or lower
                //{
                //    clipboardManager.PrimaryClip = ClipData.NewPlainText("", "");
                //}
                //else
                //{
                //    clipboardManager.ClearPrimaryClip(); // Clear the primary clipboard contents
                //}

                UpdateLastCleared();
            }
        });
    }

    public async Task<List<string?>> QuerySamsungClipboardContentsAsync()
    {
        return await Task.Run(() =>
        {
            List<string?> clipboardItems = new List<string?>();

            try
            {
                Android.Net.Uri? clipboardUri = Android.Net.Uri.Parse("content://com.samsung.android.content.clipboard/clipdata");
                if (_context.ContentResolver is null || clipboardUri is null)
                {
                    _logger.LogError("Content resolver or clipboard URI is null");
                    return clipboardItems;
                }

                using ContentProviderClient? client = _context.ContentResolver.AcquireContentProviderClient(clipboardUri);
                if (client == null) return clipboardItems;

                using Android.Database.ICursor? cursor = client.Query(
                    clipboardUri,
                    ["text", "timestamp"],
                    null,
                    null,
                    "timestamp DESC");
                if (cursor == null) return clipboardItems;

                while (cursor.MoveToNext())
                {
                    int textColumnIndex = cursor.GetColumnIndex("text");
                    if (textColumnIndex >= 0)
                    {
                        string? text = cursor.GetString(textColumnIndex);
                        clipboardItems.Add(text);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying Samsung clipboard contents");
            }

            return clipboardItems;
        });
    }

    public async Task<bool> HasClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is
                    Android.Content.ClipboardManager clipboardManager && clipboardManager.HasPrimaryClip)
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

            // Use a local variable to ensure thread safety
            EventHandler<string>? handler = _service.ClipboardContentChanged;
            handler?.Invoke(_service, content ?? string.Empty);
        }
    }
}
