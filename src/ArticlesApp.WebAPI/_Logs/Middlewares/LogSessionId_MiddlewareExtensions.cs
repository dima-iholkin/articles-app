using Microsoft.AspNetCore.Builder;



namespace ArticlesApp.WebAPI.Infrastructure.Logs;



public static class LogSessionId_MiddlewareExtensions
{
    public static IApplicationBuilder Use_LogSessionId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogSessionId_Middleware>();
    }
}