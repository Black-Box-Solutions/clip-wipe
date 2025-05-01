using Android.Content;

namespace ClipWipe.App.Services;

using ClipboardManager = Android.Content.ClipboardManager;

// ---------------------------------------
// 5. PLATFORM-SPECIFIC CODE (ANDROID)
// ---------------------------------------
public partial class ClipboardService : IClipboardService
{
    private readonly Context _context = Android.App.Application.Context;

    public async Task<string> GetClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is ClipboardManager { HasPrimaryClip: true } clipboardManager)
            {
                ClipData? clipData = clipboardManager.PrimaryClip;
                if (clipData?.ItemCount > 0)
                {
                    return clipData.GetItemAt(0)?.Text ?? string.Empty;
                }
            }

            return string.Empty;
        });
    }

    public async Task ClearClipboardAsync()
    {
        await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is ClipboardManager clipboardManager)
            {
                //TODO : Check if this is the correct way to clear the clipboard
                //TODO should this use ClipboardManager.ClearPrimaryClip?
                clipboardManager.PrimaryClip = ClipData.NewPlainText("", "");
                LastCleared = DateTime.Now;
            }
        });
    }

    public async Task<bool> HasClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            if (_context.GetSystemService(Context.ClipboardService) is ClipboardManager { HasPrimaryClip: true } clipboardManager)
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
}
