using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.WebAPI.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;



namespace ArticlesApp.WebAPI.Infrastructure.Identity;



public class UserSoftDeleted_Middleware
{
    private readonly RequestDelegate _next;

    public UserSoftDeleted_Middleware(RequestDelegate next)
    {
        _next = next;
    }



    public async Task InvokeAsync(
        HttpContext context,
        IUserRepository userRepository,
        IAuthenticationService authenticationService
    )
    {
        bool isOidcLoginRequest = context.Request.Path.Value!.StartsWith("/connect/authorize");
        if (isOidcLoginRequest)
        {
            AuthenticateResult result = await authenticationService.AuthenticateAsync(
                context,
                "Identity.Application"
            );

            if (result.Succeeded)
            {
                Claim? userIdClaim = result.Principal.Claims
                    .Where(cl => cl.Type == "sub")
                    .FirstOrDefault();

                if (userIdClaim != null)
                {
                    bool isSoftDeleted = false;
                    try
                    {
                        isSoftDeleted = await userRepository.IsSoftDeletedAsync(userIdClaim.Value);
                    }
                    catch (EntityNotFoundException)
                    {
                        // no op.
                    }

                    if (isSoftDeleted == true)
                    {
                        string redirectUrl = GetRedirectUri(context);
                        context.Response.Redirect(redirectUrl);
                        return;
                        // Redirect to the recovery page.
                        // Short-circuits the pipeline.
                    }

                }
            }
        }

        bool isIdentityRequest = context.Request.Path.Value.StartsWith("/Identity/");
        if (isIdentityRequest)
        {
            bool isLoggedIn = context.User.Identity!.IsAuthenticated;

            if (isLoggedIn)
            {
                Claim? userIdClaim = context.User.Claims
                    .Where(cl => cl.Type == "sub")
                    .FirstOrDefault();

                if (userIdClaim != null)
                {
                    bool isSoftDeleted = false;
                    try
                    {
                        isSoftDeleted = await userRepository.IsSoftDeletedAsync(userIdClaim.Value);
                    }
                    catch (EntityNotFoundException)
                    {
                        // no op.
                    }

                    if (isSoftDeleted)
                    {
                        if (
                            context.Request.Path.Value == Routes.AccountRecovery_Absolute
                            || context.Request.Path.Value == "/Identity/Account/Logout"
                        )
                        {
                            // Do nothing, as it's one of the routes allowed after the soft-delete.
                            // Pass call to the next middleware in the pipeline.
                        }
                        else
                        {
                            string redirectUrl = GetRedirectUri(context);
                            context.Response.Redirect(redirectUrl);
                            return;
                            // Redirect to the recovery page.
                            // Short-circuits the pipeline.
                        }
                    }
                }
            }
        }

        await _next(context);
    }



    private string GetRedirectUri(HttpContext context)
    {
        return $"{context.Request.Scheme}://{context.Request.Host.Value}{Routes.AccountRecovery_Absolute}";
    }
}