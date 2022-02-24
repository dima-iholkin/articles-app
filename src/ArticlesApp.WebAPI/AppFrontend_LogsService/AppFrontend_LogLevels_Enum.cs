using System.Diagnostics.CodeAnalysis;



namespace ArticlesApp.WebAPI.AppFrontend_LoggerService;



[SuppressMessage(
    "Design",
    "CA1069:Enums values should not be duplicated",
    Justification = "I think this is how the Console.Log works in the browser's JS."
)]
public enum AppFrontend_LogLevels_Enum
{
    Debug = 1,
    Verbose = 2,
    Info = 3,
    Log = 3,
    Warn = 4,
    Error = 5
}