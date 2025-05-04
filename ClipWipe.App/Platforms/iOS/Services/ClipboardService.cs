using UIKit;

namespace ClipWipe.App.Services;

// ---------------------------------------
// PLATFORM-SPECIFIC CODE (iOS)
// ---------------------------------------
public partial class ClipboardService : IClipboardService
{
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
            UIPasteboard.General.String = null;
            LastCleared = DateTime.Now;
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
