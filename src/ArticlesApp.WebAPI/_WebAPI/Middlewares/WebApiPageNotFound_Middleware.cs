using Microsoft.AspNetCore.Http;



namespace ArticlesApp.WebAPI.Infrastructure.WebAPI;



public class WebApiPageNotFound_Middleware
{
    private readonly RequestDelegate _next;

    public WebApiPageNotFound_Middleware(RequestDelegate next)
    {
        _next = next;
    }



    // Don't fall through to the SPA on "/api/" requests which didn't match any WebAPI controllers.
    // Return 404 PageNotFound.
    public async Task InvokeAsync(HttpContext context)
    {
        bool isApiRequest = context.Request.Path.Value!.StartsWith("/api/");
        if (isApiRequest)
        {
            context.Response.StatusCode = 404;
            return;
            // Return 404 PageNotFound. Short-circuits the pipeline.
        }

        await _next(context);
    }
}