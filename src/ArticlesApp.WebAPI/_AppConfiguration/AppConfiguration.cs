using ArticlesApp.WebAPI._AppConfiguration.Sections.AppRunMode;
using ArticlesApp.WebAPI._AppConfiguration.Sections.ElasticSearch;
using ArticlesApp.WebAPI._AppConfiguration.Sections.IdentityServer;
using ArticlesApp.WebAPI._AppConfiguration.Sections.UserAccounts;



namespace ArticlesApp.WebAPI._AppConfiguration;



public class AppConfiguration
{
    public AppConfiguration(
        RunModeEnum appRunMode,
        IConfiguration hostConfiguration,
        IHostEnvironment hostEnvironment,
        ElasticSearchConfiguration elasticSearch,
        IdentityServerConfiguration identityServer,
        UserAccountsConfiguration userAccounts
    )
    {
        RunMode = appRunMode;
        HostConfiguration = hostConfiguration;
        HostEnvironment = hostEnvironment;
        ElasticSearch = elasticSearch;
        IdentityServer = identityServer;
        UserAccounts = userAccounts;
    }



    public RunModeEnum RunMode { get; }

    public IConfiguration HostConfiguration { get; }
    public IHostEnvironment HostEnvironment { get; }

    public ElasticSearchConfiguration ElasticSearch { get; }

    public IdentityServerConfiguration IdentityServer { get; }

    public UserAccountsConfiguration UserAccounts { get; }



    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("AppConfiguration");

        sb.AppendLine($"{nameof(RunMode)}: {Enum.GetName(RunMode)}");
        sb.AppendLine();

        sb.AppendLine(nameof(ElasticSearch));
        sb.AppendLine($"{nameof(ElasticSearch.IsEnabled)}: {ElasticSearch.IsEnabled}");
        sb.AppendLine($"{nameof(ElasticSearch.IndexNamePrefix)}: {ElasticSearch.IndexNamePrefix}");
        sb.AppendLine();

        sb.AppendLine(nameof(IdentityServer));
        sb.AppendLine(
            $"{nameof(IdentityServer.SpaClient.AccessTokenLifetimeMinutes)}: " +
            $"{IdentityServer.SpaClient.AccessTokenLifetimeMinutes}"
        );
        sb.AppendLine();

        sb.AppendLine(nameof(UserAccounts));
        sb.AppendLine(
            $"{nameof(UserAccounts.EnableLocalAccounts)}:  " +
            $"{UserAccounts.EnableLocalAccounts}"
        );
        sb.AppendLine(
            $"{nameof(UserAccounts.SoftDeletionPeriodDays)}: " +
            $"{UserAccounts.SoftDeletionPeriodDays}"
        );

        return sb.ToString();
    }
}