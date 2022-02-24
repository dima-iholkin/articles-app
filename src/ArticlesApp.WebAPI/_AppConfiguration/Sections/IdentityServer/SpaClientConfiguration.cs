namespace ArticlesApp.WebAPI._AppConfiguration.Sections.IdentityServer;



public class SpaClientConfiguration
{
    public SpaClientConfiguration(
        string clientId,
        int accessTokenLifetimeMinutes,
        string[] redirectUris,
        string[] postLogoutRedirectUris,
        string[] additionalRoles
    )
    {
        ClientId = clientId;
        AccessTokenLifetimeMinutes = accessTokenLifetimeMinutes;
        RedirectUris = redirectUris;
        PostLogoutRedirectUris = postLogoutRedirectUris;
        AdditionalRoles = additionalRoles;
    }

    public string ClientId { get; set; }

    public int AccessTokenLifetimeMinutes { get; set; }

    public string[] RedirectUris { get; set; }
    public string[] PostLogoutRedirectUris { get; set; }

    public string[] AdditionalRoles { get; set; }
}