using UIKit;

namespace ClipWipe.App.Services;

public partial class ClipboardService : IClipboardService
{
    public async Task<string> GetClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            var pasteboard = UIPasteboard.General;
            if (pasteboard.HasStrings)
            {
                return pasteboard.String ?? string.Empty;
            }
            return string.Empty;
        });
    }

    public async Task ClearClipboardAsync()
    {
        await Task.Run(() =>
        {
            UIPasteboard.General.String = string.Empty;
            LastCleared = DateTime.Now;
        });
    }

    public async Task<bool> HasClipboardContentAsync()
    {
        return await Task.Run(() =>
        {
            var pasteboard = UIPasteboard.General;
            return pasteboard.HasStrings && !string.IsNullOrEmpty(pasteboard.String);
        });
    }
}
