namespace ClipWipe.App.Services;

// ---------------------------------------
// PLATFORM-SPECIFIC CODE (Windows)
// ---------------------------------------
public partial class ClipboardService : IClipboardService
{
    public Task<string> GetClipboardContentAsync()
    {
        throw new NotImplementedException();
    }

    public Task ClearClipboardAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasClipboardContentAsync()
    {
        throw new NotImplementedException();
    }
}
