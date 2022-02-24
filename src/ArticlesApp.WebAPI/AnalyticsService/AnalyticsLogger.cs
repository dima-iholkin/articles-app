using ArticlesApp.WebAPI.AnalyticsService.Events;
using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions.Enums;
using Serilog;
using Serilog.Core;



namespace ArticlesApp.WebAPI.AnalyticsService;



public class AnalyticsLogger
{
    private readonly Logger _logger;

    public AnalyticsLogger(AppConfiguration appConfiguration)
    {
        var frontendLoggerConfig = new LoggerConfiguration()
            .ConfigureLogger(
                appConfiguration,
                LoggerAreaEnum.AppBackend,
                LoggerTypeEnum.Analytics
            );

        _logger = frontendLoggerConfig.CreateLogger();
    }



    public void LogResponseInfo(ResponseInfo responseInfo)
    {
        _logger.Information(
            "{@ResponseInfo}",
            responseInfo
        );
    }
}