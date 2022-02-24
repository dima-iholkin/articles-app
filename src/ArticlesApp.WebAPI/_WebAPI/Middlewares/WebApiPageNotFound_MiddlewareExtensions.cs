using Microsoft.AspNetCore.Builder;



namespace ArticlesApp.WebAPI.Infrastructure.WebAPI;



public static class WebApiPageNotFound_MiddlewareExtensions
{
    public static IApplicationBuilder UseWebApiPageNotFound(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<WebApiPageNotFound_Middleware>();
    }
}