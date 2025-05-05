namespace ClipWipe.App.Services;

/// <summary>
/// Platform-agnostic implementation for clipboard operations.
/// </summary>
public partial class ClipboardService : IClipboardService
{
    // This partial class will be implemented differently for each platform
    // We'll define the common properties here

    public DateTimeOffset? LastCleared { get; private set; }

    // The platform-specific implementations will be in Platform folders
}
