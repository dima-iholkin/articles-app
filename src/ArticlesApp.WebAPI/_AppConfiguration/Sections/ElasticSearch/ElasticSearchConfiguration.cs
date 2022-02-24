using Serilog.Sinks.Elasticsearch;



namespace ArticlesApp.WebAPI._AppConfiguration.Sections.ElasticSearch;



public class ElasticSearchConfiguration
{
    public ElasticSearchConfiguration(
        bool isEnabled,
        string indexNamePrefix,
        ElasticsearchSinkOptions sinkOptions
    )
    {
        IsEnabled = isEnabled;
        IndexNamePrefix = indexNamePrefix;
        ElasticsearchSinkOptions = sinkOptions;
    }



    public bool IsEnabled { get; }
    public string IndexNamePrefix { get; }
    public ElasticsearchSinkOptions ElasticsearchSinkOptions { get; }
}