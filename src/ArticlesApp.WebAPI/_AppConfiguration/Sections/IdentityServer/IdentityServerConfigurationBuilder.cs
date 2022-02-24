namespace ArticlesApp.WebAPI._AppConfiguration.Sections.IdentityServer;



public class IdentityServerConfigurationBuilder
{
    public static IdentityServerConfiguration Build(IConfiguration hostConfig)
    {
        string routeSPA = $"Custom:IdentityServer:SPA";

        string clientId = hostConfig.GetValue<string?>(
            $"{routeSPA}:{nameof(SpaClientConfiguration.ClientId)}"
        ) ?? "ArticlesApp.WebAPI";

        int accessTokenLifetimeMinutes = hostConfig.GetValue<int?>(
            $"{routeSPA}:{nameof(SpaClientConfiguration.AccessTokenLifetimeMinutes)}"
        ) ?? 15;

        string[] redirectUris = hostConfig.
            GetSection(
                $"{routeSPA}:{nameof(SpaClientConfiguration.RedirectUris)}"
            )
            .Get<string[]>();

        string[] postLogoutRedirectUris = hostConfig
            .GetSection(
                $"{routeSPA}:{nameof(SpaClientConfiguration.PostLogoutRedirectUris)}"
            )
            .Get<string[]>();

        string[] additionalRoles = hostConfig
            .GetSection(
                $"{routeSPA}:{nameof(SpaClientConfiguration.AdditionalRoles)}"
            )
            .Get<string[]>();

        SpaClientConfiguration spaConfiguration = new SpaClientConfiguration(
            clientId,
            accessTokenLifetimeMinutes,
            redirectUris,
            postLogoutRedirectUris,
            additionalRoles
        );

        return new IdentityServerConfiguration(spaConfiguration);
    }
}