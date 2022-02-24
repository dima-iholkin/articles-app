using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;



namespace ArticlesApp.WebAPI.IdentityService.Services;



public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(
        string name,
        JwtBearerOptions options
    )
    {
        Func<MessageReceivedContext, Task> originalOnMessageReceived = options.Events.OnMessageReceived;
        options.Events.OnMessageReceived = async context =>
        {
            await originalOnMessageReceived(context);

            if (string.IsNullOrEmpty(context.Token))
            {
                StringValues accessToken = context.Request.Query["access_token"];
                PathString path = context.HttpContext.Request.Path;

                if (
                    !string.IsNullOrEmpty(accessToken)
                    && (
                        path.StartsWithSegments("/api/notificationsHub")
                        || path.StartsWithSegments("/api/myArticlesHub")
                        || path.StartsWithSegments("/api/moderatorArticlesHub")
                    )
                )
                {
                    context.Token = accessToken;
                }
            }
        };
    }
}