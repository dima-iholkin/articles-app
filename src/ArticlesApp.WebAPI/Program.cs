using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ArticlesApp.Core.SoftDeletionReaper;
using ArticlesApp.Database.SqlServer.Dev_DbInitializer;
using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI._AppConfiguration.Sections.AppRunMode;
using ArticlesApp.WebAPI.EventBrokerSubscriptions;
using ArticlesApp.WebAPI.Helpers.LoggerExtensions;
using Microsoft.AspNetCore.Hosting;
using Serilog;



namespace ArticlesApp.WebAPI;



public class Program
{
    public static void Main(string[] args)
    {
        PrintProcessInformation();

        IHost host = CreateHostBuilder(args)
            .Build();

        PrintAppConfiguration(host);
        CheckHttpsCertificate(host);
        //host.ValidateAppConfiguration();

        RegisterEventHandlers(host);

        if (TryToRunInFillOrReaperMode(host) == true)
        {
            return;
            // Don't start the web server in this mode.
            // Exit the application after the database operations finish.
        }

        // Start the web server:
        try
        {
            host.Run();
            return;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application terminated unexpectedly.");
            return;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }



    private static void CheckHttpsCertificate(IHost host)
    {
        AppConfiguration appConfig = host.Services.GetRequiredService<AppConfiguration>();
        if (appConfig.HostEnvironment.IsDevelopment())
        {
            return;
        }

        string certPath = appConfig.HostConfiguration["Kestrel:Endpoints:Https:Certificate:Path"]
            ?? throw new Exception("HTTPS certificate path was not found in the configuration.");
        string certPassword = appConfig.HostConfiguration["Kestrel:Endpoints:Https:Certificate:Password"]
            ?? throw new Exception("HTTPS certificate password was not found in the configuration.");
        try
        {
            X509Certificate2 cert = new X509Certificate2(
                certPath,
                certPassword
            );
        }
        catch (CryptographicException)
        {
            ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError("HTTPS certificate {certificatePath} and password did not match.", certPath);

            throw;
        }
    }



    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder hostBuilder = Host
            .CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configBuilder =>
            {
                configBuilder.AddEnvironmentVariables(prefix: "ARTICLESAPP_");

                string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (env != null)
                {
                    configBuilder.AddEnvironmentVariables(prefix: $"ARTICLESAPP_{env.ToUpper()}_");
                }
            })
            .ConfigureLogger()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        return hostBuilder;
    }



    private static void PrintAppConfiguration(IHost host)
    {
        AppConfiguration appConfig = host.Services.GetRequiredService<AppConfiguration>();

        Console.WriteLine(appConfig.ToString());
    }



    private static void PrintProcessInformation()
    {
        Console.WriteLine("MachineName: " + Environment.MachineName);
        Console.WriteLine("ProcessOwner: " + Environment.UserDomainName + "\\" + Environment.UserName);
        Console.WriteLine(
            "AppData path: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        );
        Console.WriteLine("Environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        Console.WriteLine();
    }



    private static void RegisterEventHandlers(IHost host)
    {
        NotificationSubscriptions notificationsEventSubscriptions =
            host.Services.GetRequiredService<NotificationSubscriptions>();
        notificationsEventSubscriptions.RegisterSubscriptions();

        ArticleSubscriptions articleEventSubscriptions =
            host.Services.GetRequiredService<ArticleSubscriptions>();
        articleEventSubscriptions.RegisterSubscriptions();
    }



    private static bool TryToRunInFillOrReaperMode(IHost host)
    {
        AppConfiguration appConfig = host.Services.GetRequiredService<AppConfiguration>();

        switch (appConfig.RunMode)
        {
            case RunModeEnum.Normal:
                return false;
            case RunModeEnum.DbInitializer:
                host.RunDbInitialization();
                return true;
            case RunModeEnum.DbReaper:
                SoftDeletionReaperExtensions.RunReaperMode(
                    host,
                    appConfig.UserAccounts.SoftDeletionPeriodDays
                );
                return true;
            default:
                throw new Exception($"Unexpected value of {appConfig.RunMode}.");
        }
    }
}