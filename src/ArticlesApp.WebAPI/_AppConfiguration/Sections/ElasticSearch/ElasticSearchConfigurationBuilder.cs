using ArticlesApp.WebAPI._AppSecrets;
using Serilog.Sinks.Elasticsearch;



namespace ArticlesApp.WebAPI._AppConfiguration.Sections.ElasticSearch;



public static class ElasticSearchConfigurationBuilder
{
    public static ElasticSearchConfiguration Build(
        IConfiguration hostConfig,
        AppSecrets appSecrets
    )
    {
        string route = "Custom:ElasticSearch";

        bool isEnabled = hostConfig.GetValue<bool?>(
            $"{route}:{nameof(ElasticSearchConfiguration.IsEnabled)}"
        ) ?? true;

        string indexNamePrefix = hostConfig.GetValue<string>(
            $"{route}:{nameof(ElasticSearchConfiguration.IndexNamePrefix)}"
        ) ?? "articlesapp-default";

        Uri hostUri = new Uri(appSecrets.ElasticSearch_HostUri);
        ElasticsearchSinkOptions sinkOptions = new ElasticsearchSinkOptions(hostUri)
        {
            AutoRegisterTemplate = hostConfig.GetValue<bool?>(
                $"{route}:{nameof(ElasticsearchSinkOptions.AutoRegisterTemplate)}"
            ) ?? true,
            AutoRegisterTemplateVersion = hostConfig.GetValue<AutoRegisterTemplateVersion?>(
                $"{route}:{nameof(ElasticsearchSinkOptions.AutoRegisterTemplateVersion)}"
            ) ?? AutoRegisterTemplateVersion.ESv7,
            NumberOfShards = hostConfig.GetValue<int?>(
                $"{route}:{nameof(ElasticsearchSinkOptions.NumberOfShards)}"
            ) ?? 1,
            NumberOfReplicas = hostConfig.GetValue<int?>(
                $"{route}:{nameof(ElasticsearchSinkOptions.NumberOfReplicas)}"
            ) ?? 0
        };

        return new ElasticSearchConfiguration(
            isEnabled,
            indexNamePrefix,
            sinkOptions
        );
    }
}