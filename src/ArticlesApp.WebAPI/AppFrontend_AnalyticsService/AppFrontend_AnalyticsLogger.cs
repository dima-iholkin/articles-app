using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI.AppFrontend_AnalyticsService.Events;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions.Enums;
using Serilog;
using Serilog.Core;



namespace ArticlesApp.WebAPI.AppFrontend_AnalyticsService;



public class AppFrontend_AnalyticsLogger
{
    private readonly Logger _logger;

    public AppFrontend_AnalyticsLogger(AppConfiguration appConfiguration)
    {
        var frontendLoggerConfig = new LoggerConfiguration()
            .ConfigureLogger(
                appConfiguration,
                LoggerAreaEnum.AppFrontend,
                LoggerTypeEnum.Analytics
            );

        _logger = frontendLoggerConfig.CreateLogger();
    }



    public void Log_RouteChange(AppFrontend_RouteChange ev)
    {
        _logger.Information("{@RouteChange}", ev);
    }
}