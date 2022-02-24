using Microsoft.AspNetCore.Mvc;



namespace ArticlesApp.WebAPI.AppFrontend_LoggerService;



[Route("api/logs")]
[ApiController]
public class AppFrontend_LogsController : ControllerBase
{
    private readonly Serilog.ILogger _logger;

    public AppFrontend_LogsController(AppFrontend_LogsSingleton loggerSingleton) : base()
    {
        _logger = loggerSingleton.GetLogger();
    }



    [HttpPost]
    public ActionResult PostLogs([FromBody] AppFrontend_Log[] logs)
    {
        HttpContext.Request.Cookies.TryGetValue(
            "ClientLoggingId",
            out string? clientLoggingId
        );

        foreach (AppFrontend_Log log in logs.Reverse())
        {
            switch (log.LogLevel)
            {
                case AppFrontend_LogLevels_Enum.Info:
                    _logger.Information(
                        "AppFrontend log message: {Message}. SessionId: {SessionId}",
                        log.Message,
                        clientLoggingId
                    );
                    break;
                case AppFrontend_LogLevels_Enum.Warn:
                    _logger.Warning(
                        "AppFrontend log message: {Message}. Stack: {Stack}. SessionId: {SessionId}",
                        log.Message,
                        log.Stack,
                        clientLoggingId
                    );
                    break;
                case AppFrontend_LogLevels_Enum.Error:
                    _logger.Error(
                        "AppFrontend log message: {Message}. Stack: {Stack}. SessionId: {SessionId}",
                        log.Message,
                        log.Stack,
                        clientLoggingId
                    );
                    break;
                default:
                    break;
            }
        }
        // Recurse the logs array from the tail.
        // because this is the way the AppFrontend logger sends the data.

        return Ok();
    }
    // POST: api/logs
}