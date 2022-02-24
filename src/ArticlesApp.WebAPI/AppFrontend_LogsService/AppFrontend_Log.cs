namespace ArticlesApp.WebAPI.AppFrontend_LoggerService;



public class AppFrontend_Log
{
    public string? Message { get; set; }
    public string? Stack { get; set; }
    public AppFrontend_LogLevels_Enum LogLevel { get; set; }
}