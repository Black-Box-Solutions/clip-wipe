namespace ClipWipe.App.Services;

// ---------------------------------------
// PLATFORM-SPECIFIC CODE (Windows)
// ---------------------------------------
public partial class ClipboardService : IClipboardService
{
    public async Task<string?> GetClipboardContentAsync()
    {
        if (Clipboard.HasText)
        {
            return await Clipboard.GetTextAsync();
        }
        return null;
    }

    public async Task ClearClipboardAsync()
    {
        await Clipboard.SetTextAsync(null);
        LastCleared = DateTime.Now;
    }

    public async Task<bool> HasClipboardContentAsync()
    {
        return await Task.FromResult(Clipboard.HasText);
    }
}
