using ArticlesApp.Core.Entities.Identity;



namespace ArticlesApp.WebAPI.Helpers;



public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }



    public static IdentityRolesEnum[] GetRoles(this ClaimsPrincipal user)
    {
        IEnumerable<Claim> claims = user.FindAll(ClaimTypes.Role);
        IEnumerable<string> roleStrings = claims.Select(cl => cl.Value);

        List<IdentityRolesEnum> roles = new List<IdentityRolesEnum>();
        foreach (string roleStr in roleStrings)
        {
            bool success = Enum.TryParse<IdentityRolesEnum>(
                roleStr,
                ignoreCase: true,
                out IdentityRolesEnum roleEnum
            );

            if (success)
            {
                roles.Add(roleEnum);
            }
        }

        return roles.ToArray();
    }
}