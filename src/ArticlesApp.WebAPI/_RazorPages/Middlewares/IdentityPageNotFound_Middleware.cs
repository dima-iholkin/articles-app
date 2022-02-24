using Microsoft.AspNetCore.Http;



namespace ArticlesApp.WebAPI._RazorPages.Middlewares;



public class IdentityPageNotFound_Middleware
{
    private readonly RequestDelegate _next;

    public IdentityPageNotFound_Middleware(RequestDelegate next)
    {
        _next = next;
    }



    // Don't fall through to the SPA on "/Identity/" requests which didn't match any RazorPages controllers.
    // Return 404 PageNotFound.
    public async Task InvokeAsync(HttpContext context)
    {
        bool isIdentityPageRequest = context.Request.Path.Value!.StartsWith("/Identity/");
        if (isIdentityPageRequest)
        {
            context.Response.StatusCode = 404;
            return;
            // Return 404 PageNotFound.
            // Short-circuits the pipeline.
        }

        await _next(context);
    }
}