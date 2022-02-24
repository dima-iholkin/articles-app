using Microsoft.AspNetCore.Http;
using Serilog.Context;



namespace ArticlesApp.WebAPI.Infrastructure.Logs;



public class LogSessionId_Middleware
{
    private readonly RequestDelegate _next;

    public LogSessionId_Middleware(RequestDelegate next)
    {
        _next = next;
    }



    public async Task InvokeAsync(HttpContext context)
    {
        bool success = context.Request.Cookies.TryGetValue(
            "ClientLoggingId",
            out string? clientLoggingId
        );

        if (success)
        {
            LogContext.PushProperty(
                "SessionId",
                clientLoggingId
            );
        }

        await _next(context);
    }
    // Add the SessionId to the logs produced.
}