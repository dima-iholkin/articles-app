using ArticlesApp.WebAPI._AppConfiguration.Sections.ElasticSearch;



namespace ArticlesApp.WebAPI._AppConfiguration;



public static class AppConfigurationValidator
{
    public static AppConfiguration Validate(this AppConfiguration appConfiguration)
    {
        bool esIsValidated = ValidateElasticSearch(appConfiguration.ElasticSearch);
        if (esIsValidated == false)
        {
            throw new Exception("ElasticSearch configuration failed validation.");
        }

        return appConfiguration;
    }



    private static bool ValidateElasticSearch(ElasticSearchConfiguration esConfig)
    {
        if (esConfig.IsEnabled == false)
        {
            return true;
        }

        if (esConfig.IndexNamePrefix.IsPresent() == false)
        {
            return false;
        }

        return true;
    }
}