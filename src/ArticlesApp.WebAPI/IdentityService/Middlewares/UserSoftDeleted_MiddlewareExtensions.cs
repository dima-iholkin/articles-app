using Microsoft.AspNetCore.Builder;



namespace ArticlesApp.WebAPI.Infrastructure.Identity;



public static class UserSoftDeleted_MiddlewareExtensions
{
    public static IApplicationBuilder UseUserSoftDeleted(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserSoftDeleted_Middleware>();
    }
}