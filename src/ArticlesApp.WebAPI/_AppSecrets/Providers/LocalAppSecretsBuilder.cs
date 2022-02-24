using ArticlesApp.WebAPI._AppSecrets.Model;

namespace ArticlesApp.WebAPI._AppSecrets.Providers;



public static class LocalAppSecretsBuilder
{
    public static AppSecrets Build(IConfiguration hostConfig)
    {
        IConfigurationRoot secretsHostConfig = new ConfigurationBuilder()
            .AddConfiguration(hostConfig)
            .AddUserSecrets<Startup>()
            .Build();

        LoginProviderCredentials _githubCredentials = new LoginProviderCredentials(
            secretsHostConfig["Custom:IdentityServer:Providers:GitHub:ClientId"],
            secretsHostConfig["Custom:IdentityServer:Providers:GitHub:ClientSecret"]
        );
        LoginProviderCredentials _googleCredentials = new LoginProviderCredentials(
            secretsHostConfig["Custom:IdentityServer:Providers:Google:ClientId"],
            secretsHostConfig["Custom:IdentityServer:Providers:Google:ClientSecret"]
        );

        return new AppSecrets(
            dbConnectionString: secretsHostConfig[$"DbConnectionString"],
            githubCredentials: _githubCredentials,
            googleCredentials: _googleCredentials,
            elasticSearch_HostUri: secretsHostConfig["Custom:ElasticSearch:HostUri"] 
                ?? "http://localhost:9200",
            signalR_Redis_ConnectionString: 
                secretsHostConfig["Custom:SignalR_Redis:ConnectionString"]  
                ?? "http://localhost:6379"
        );
    }
}