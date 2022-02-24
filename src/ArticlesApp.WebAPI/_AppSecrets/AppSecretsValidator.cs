namespace ArticlesApp.WebAPI._AppSecrets;



public static class AppSecretsValidator
{
    public static AppSecrets Validate(this AppSecrets appSecrets)
    {
        if (appSecrets.DbConnectionString.IsPresent() == false)
        {
            throw new Exception(
                $"{nameof(appSecrets.DbConnectionString)} configuration failed validation."
            );
        }

        if (
            appSecrets.GitHubCredentials.ClientId.IsPresent() == false
            || appSecrets.GitHubCredentials.ClientSecret.IsPresent() == false
        )
        {
            throw new Exception("GitHub login provider configuration validation failed.");
        }

        if (
            appSecrets.GoogleCredentials.ClientId.IsPresent() == false
            || appSecrets.GoogleCredentials.ClientSecret.IsPresent() == false
        )
        {
            throw new Exception("Google login provider configuration validation failed.");
        }

        return appSecrets;
    }
}