using ArticlesApp.WebAPI._AppSecrets.Model;



namespace ArticlesApp.WebAPI._AppSecrets;



public class AppSecrets
{
    public AppSecrets(
        string dbConnectionString,
        LoginProviderCredentials githubCredentials,
        LoginProviderCredentials googleCredentials,
        string elasticSearch_HostUri,
        string signalR_Redis_ConnectionString
    )
    {
        DbConnectionString = dbConnectionString;
        GitHubCredentials = githubCredentials;
        GoogleCredentials = googleCredentials;
        ElasticSearch_HostUri = elasticSearch_HostUri;
        SignalR_Redis_ConnectionString = signalR_Redis_ConnectionString;
    }



    public string DbConnectionString { get; }

    public LoginProviderCredentials GitHubCredentials { get; }
    public LoginProviderCredentials GoogleCredentials { get; }

    public string ElasticSearch_HostUri { get; }

    public string SignalR_Redis_ConnectionString { get; }
}