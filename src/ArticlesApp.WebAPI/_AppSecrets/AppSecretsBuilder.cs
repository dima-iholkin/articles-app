using ArticlesApp.WebAPI._AppSecrets.Providers;



namespace ArticlesApp.WebAPI._AppSecrets;



public class AppSecretsBuilder
{
    private readonly IConfiguration _hostConfig;

    public AppSecretsBuilder(IConfiguration hostConfig)
    {
        _hostConfig = hostConfig;
    }



    public AppSecrets Build()
    {
        SecretsSourceEnum secretsSource = _hostConfig.GetValue<SecretsSourceEnum>("SecretsSource");

        AppSecrets appSecrets = secretsSource switch
        {
            SecretsSourceEnum.Local => BuildFromLocalSecrets(),
            SecretsSourceEnum.GCP => throw new NotImplementedException("Need to implement to run on GCP."),
            _ => throw new Exception($"SecretsSource unexpected configuration value: {secretsSource}."),
        };

        return appSecrets;
    }



    private AppSecrets BuildFromLocalSecrets()
    {
        AppSecrets appSecrets = LocalAppSecretsBuilder.Build(_hostConfig);

        return appSecrets;
    }
}