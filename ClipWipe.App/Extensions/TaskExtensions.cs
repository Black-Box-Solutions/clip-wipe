namespace ClipWipe.App.Extensions;

using ClipWipe.App.Handlers;

public static class TaskExtensions
{
    /// <summary>
    /// Fire and Forget Safe Async.
    /// </summary>
    /// <param name="task">Task to Fire and Forget.</param>
    /// <param name="handler">Error Handler.</param>
    //TODO is this really safe to have async void?  look at what i did in the PoolTracker app
    public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler? handler = null)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            handler?.HandleError(ex);
        }
    }
}
