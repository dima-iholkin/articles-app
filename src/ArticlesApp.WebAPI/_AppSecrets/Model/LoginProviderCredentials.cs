namespace ArticlesApp.WebAPI._AppSecrets.Model;



public class LoginProviderCredentials
{
    public LoginProviderCredentials(
        string clientId,
        string clientSecret
    )
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
    }



    public string ClientId { get; }
    public string ClientSecret { get; }
}