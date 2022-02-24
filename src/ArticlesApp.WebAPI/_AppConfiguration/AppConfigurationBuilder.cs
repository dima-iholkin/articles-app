using ArticlesApp.WebAPI._AppConfiguration.Sections.AppRunMode;
using ArticlesApp.WebAPI._AppConfiguration.Sections.ElasticSearch;
using ArticlesApp.WebAPI._AppConfiguration.Sections.IdentityServer;
using ArticlesApp.WebAPI._AppConfiguration.Sections.UserAccounts;
using ArticlesApp.WebAPI._AppSecrets;



namespace ArticlesApp.WebAPI._AppConfiguration;



public static class AppConfigurationBuilder
{
    public static AppConfiguration Build(
        IHostEnvironment hostEnv,
        IConfiguration hostConfig,
        AppSecrets appSecrets
    )
    {
        RunModeEnum runMode = RunModeBuilder.Build(hostConfig);

        ElasticSearchConfiguration esConfig = ElasticSearchConfigurationBuilder.Build(
            hostConfig,
            appSecrets
        );

        IdentityServerConfiguration isConfig = IdentityServerConfigurationBuilder.Build(hostConfig);

        UserAccountsConfiguration userAccountsConfig = UserAccountsConfigurationBuilder.Build(hostConfig);

        return new AppConfiguration(
            runMode,
            hostConfig,
            hostEnv,
            esConfig,
            isConfig,
            userAccountsConfig
        );
    }
}