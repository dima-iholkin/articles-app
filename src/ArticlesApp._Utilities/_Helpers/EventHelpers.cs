using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;



namespace ArticlesApp._Utilities._Helpers;



public static class EventHelpers
{
    public static void FireAndForget(
        Func<Task> action,
        ILogger logger
    )
    {
        Task.Run(async () =>
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    exception: ex,
                    message: ex.Message
                );
            }
        });
    }
}