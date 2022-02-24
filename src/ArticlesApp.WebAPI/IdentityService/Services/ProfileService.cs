using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;



namespace ArticlesApp.WebAPI.IdentityService;



public class ProfileService : IProfileService
{
    public ProfileService()
    { }



    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        IEnumerable<Claim> roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);

        context.IssuedClaims.AddRange(roleClaims);

        return Task.CompletedTask;
    }
    // Add the role claim when creating the OpenID Connect token.



    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}