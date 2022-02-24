using Microsoft.AspNetCore.Builder;



namespace ArticlesApp.WebAPI._RazorPages.Middlewares;



public static class IdentityPageNotFound_MiddlewareExtensions
{
    public static IApplicationBuilder UseIdentityPageNotFound(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IdentityPageNotFound_Middleware>();
    }
}