using Microsoft.AspNetCore.Builder;



namespace ArticlesApp.WebAPI.AnalyticsService;



public static class AnalyticsLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseInfoAnalytics(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AnalyticsLoggerMiddleware_LogResponseInfo>();
    }
}