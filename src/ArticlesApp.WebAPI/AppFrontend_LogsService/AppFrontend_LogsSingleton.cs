using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions.Enums;
using Serilog;
using Serilog.Core;



namespace ArticlesApp.WebAPI.AppFrontend_LoggerService;



public class AppFrontend_LogsSingleton
{
    private readonly Logger _logger;

    public AppFrontend_LogsSingleton(AppConfiguration appConfiguration)
    {
        var frontendLoggerConfig = new LoggerConfiguration()
            .ConfigureLogger(
                appConfiguration,
                LoggerAreaEnum.AppFrontend,
                LoggerTypeEnum.Logs
            );

        _logger = frontendLoggerConfig.CreateLogger();
    }



    public Logger GetLogger()
    {
        return _logger;
    }
}