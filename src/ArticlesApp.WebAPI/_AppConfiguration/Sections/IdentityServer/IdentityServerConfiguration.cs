namespace ArticlesApp.WebAPI._AppConfiguration.Sections.IdentityServer;



public class IdentityServerConfiguration
{
    public IdentityServerConfiguration(SpaClientConfiguration spaClientConfiguration)
    {
        SpaClient = spaClientConfiguration;
    }



    public SpaClientConfiguration SpaClient { get; set; }
}