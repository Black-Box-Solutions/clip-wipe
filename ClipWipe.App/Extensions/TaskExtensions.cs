using ClipWipe.App.Handlers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ClipWipe.App.Extensions;

public static class TaskExtensions
{
    //TODO consider using https://github.com/TheCodeTraveler/AsyncAwaitBestPractices or https://www.nuget.org/packages/AsyncAwaitBestPractices.MVVM/

    /// <summary>
    /// Fire and Forget Safe Async.
    /// </summary>
    /// <param name="task">Task to Fire and Forget.</param>
    /// <param name="errorHandler">Optional error handler.</param>
    /// <param name="logger">Optional logger.</param>
    public static void SafeFireAndForget(this Task task, IErrorHandler? errorHandler = null, ILogger? logger = null)
    {
        ArgumentNullException.ThrowIfNull(task);

        // Use the TaskScheduler.Default to ensure the continuation runs on a thread pool thread
        // Use the Forget pattern to avoid unobserved task exceptions
        _ = task.ContinueWith(t =>
        {
            try
            {
                if (t.IsFaulted && t.Exception is not null)
                {
                    Exception ex = t.Exception.Flatten().InnerException ?? t.Exception;
                    errorHandler?.HandleError(ex);
                    logger?.LogError(ex, "Unhandled exception in fire-and-forget task.");
                    Debug.WriteLine($"[SafeFireAndForget] {ex}");
                }
            }
            catch (Exception continuationEx)
            {
                // Last-resort fallback logging
                Debug.WriteLine($"[SafeFireAndForget CONTINUATION ERROR] {continuationEx}");
                logger?.LogError(continuationEx, "Exception occurred in fire-and-forget error handler.");
            }
        }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
    }
}
