using ArticlesApp.Database.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;



namespace ArticlesApp.WebAPI.IdentityService;



public class RefreshTokenService : DefaultRefreshTokenService
{
    private readonly UserManager<ApplicationUser_DB> _userManager;

    public RefreshTokenService(
        IRefreshTokenStore refreshTokenStore,
        IProfileService profile,
        ISystemClock clock,
        ILogger<DefaultRefreshTokenService> logger,
        UserManager<ApplicationUser_DB> userManager
    ) : base(
        refreshTokenStore,
        profile,
        clock,
        logger
    )
    {
        _userManager = userManager;
    }



    public override async Task<TokenValidationResult> ValidateRefreshTokenAsync(
        string token,
        Client client
    )
    {
        RefreshToken refreshToken = await this.RefreshTokenStore.GetRefreshTokenAsync(token);

        ClaimsPrincipal userPrincipal = refreshToken.Subject;
        ApplicationUser_DB user = await _userManager.GetUserAsync(userPrincipal);

        if (user == null)
        {
            return new TokenValidationResult()
            {
                IsError = true,
                Error = "User was not found."
            };
        }

        if (user.SoftDeletedAt_DateUtc != null)
        {
            return new TokenValidationResult()
            {
                IsError = true,
                Error = "User was soft-deleted."
            };
        }
        // If the user is soft-deleted, the access_token shouldn't be refreshed.

        TokenValidationResult tokenValidationResult = await base.ValidateRefreshTokenAsync(
            token,
            client
        );

        return tokenValidationResult;
    }
}