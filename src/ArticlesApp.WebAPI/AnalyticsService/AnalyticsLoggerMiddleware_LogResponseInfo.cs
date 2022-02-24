using ArticlesApp.WebAPI.AnalyticsService.Events;
using Microsoft.AspNetCore.Http;



namespace ArticlesApp.WebAPI.AnalyticsService;



public class AnalyticsLoggerMiddleware_LogResponseInfo
{
    private readonly RequestDelegate _next;

    public AnalyticsLoggerMiddleware_LogResponseInfo(RequestDelegate next)
    {
        _next = next;
    }



    public async Task InvokeAsync(
        HttpContext context,
        AnalyticsLogger analyticsLogger
    )
    {
        await _next(context);

        context.Request.Cookies.TryGetValue(
            "ClientLoggingId", 
            out string? clientLoggingId
        );
        
        var responseInfo = new ResponseInfo
        {
            ClientLoggingId = clientLoggingId,
            Path = context.Request.Path,
            Method = context.Request.Method,
            Host = context.Request.Host.Value,
            Scheme = context.Request.Scheme,
            ResponseCode = context.Response.StatusCode
        };

        analyticsLogger.LogResponseInfo(responseInfo);
    }
}