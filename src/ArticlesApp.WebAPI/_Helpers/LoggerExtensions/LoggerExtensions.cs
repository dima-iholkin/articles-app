using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions.Enums;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Templates;
using Serilog.Templates.Themes;



namespace ArticlesApp.WebAPI.Helpers.LoggerExtensions;



public static class LoggerExtensions
{
    public static IHostBuilder ConfigureLogger(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog(ConfigureLogger)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSerilog();
            });
    }



    private static void ConfigureLogger(
        HostBuilderContext hostContext,
        IServiceProvider serviceProvider,
        LoggerConfiguration loggerConfiguration
    )
    {
        AppConfiguration appConfiguration = serviceProvider.GetRequiredService<AppConfiguration>();

        loggerConfiguration.ConfigureLogger(
            appConfiguration,
            LoggerAreaEnum.AppBackend,
            LoggerTypeEnum.Logs
        );
    }



    public static LoggerConfiguration ConfigureLogger(
        this LoggerConfiguration loggerConfiguration,
        AppConfiguration appConfiguration,
        LoggerAreaEnum loggerArea,
        LoggerTypeEnum loggerType
    )
    {
        loggerConfiguration
            .ReadFrom.Configuration(appConfiguration.HostConfiguration);

        if (loggerType == LoggerTypeEnum.Logs)
        {
            if (appConfiguration.HostEnvironment.IsDevelopment())
            {
                loggerConfiguration
                    .WriteTo.Console(new ExpressionTemplate(
                        "[{@t:HH:mm:ss} {@l:u3}] " +
                            "[{Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}] {@m}\n{@x}",
                        theme: TemplateTheme.Literate
                    ));
            }
            else
            {
                loggerConfiguration
                    .WriteTo.Console(new ExpressionTemplate(
                        "[{@t:HH:mm:ss} {@l:u3}] " +
                            "[{Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)}] {@m}\n{@x}",
                        theme: TemplateTheme.Literate
                    ));
            }

            loggerConfiguration
                .Enrich.FromLogContext();
        }

        if (loggerType == LoggerTypeEnum.Analytics)
        {
            loggerConfiguration
                .MinimumLevel.Information();
        }

        if (appConfiguration.ElasticSearch.IsEnabled)
        {
            string area = loggerArea switch
            {
                LoggerAreaEnum.AppBackend => "appbackend",
                LoggerAreaEnum.AppFrontend => "appfrontend",
                _ => throw new ArgumentException("Unexpected enum value.", nameof(loggerArea)),
            };

            string type = loggerType switch
            {
                LoggerTypeEnum.Logs => "logs",
                LoggerTypeEnum.Analytics => "analytics",
                _ => throw new ArgumentException("Unexpected enum value.", nameof(loggerType)),
            };

            ElasticsearchSinkOptions esOptions = appConfiguration.ElasticSearch.ElasticsearchSinkOptions;
            esOptions.IndexFormat =
                $"{appConfiguration.ElasticSearch.IndexNamePrefix}-{area}-{type}-{DateTime.UtcNow:yyyy.MM.dd}";

            loggerConfiguration
                .WriteTo.Elasticsearch(esOptions);
        }

        return loggerConfiguration;
    }
}