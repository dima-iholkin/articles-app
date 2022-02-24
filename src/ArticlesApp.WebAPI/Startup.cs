using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications;
using ArticlesApp.Core.Orchestrators.OrchestratorSubscribers;
using ArticlesApp.Core.SoftDeletionReaper;
using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer;
using ArticlesApp.Database.SqlServer.Converters;
using ArticlesApp.Database.SqlServer.Dev_DbInitializer;
using ArticlesApp.Database.SqlServer.Repositories;
using ArticlesApp.EventBroker.LocalCaller;
using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI._AppConfiguration.Sections.IdentityServer;
using ArticlesApp.WebAPI._AppSecrets;
using ArticlesApp.WebAPI._RazorPages.Middlewares;
using ArticlesApp.WebAPI.AnalyticsService;
using ArticlesApp.WebAPI.AppFrontend_AnalyticsService;
using ArticlesApp.WebAPI.AppFrontend_LoggerService;
using ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Helpers;
using ArticlesApp.WebAPI.ArticlesService;
using ArticlesApp.WebAPI.EventBrokerSubscriptions;
using ArticlesApp.WebAPI.Helpers;
using ArticlesApp.WebAPI.IdentityService;
using ArticlesApp.WebAPI.IdentityService.Services;
using ArticlesApp.WebAPI.IdentityService.Services.AccountMerge;
using ArticlesApp.WebAPI.Infrastructure.Identity;
using ArticlesApp.WebAPI.Infrastructure.Logs;
using ArticlesApp.WebAPI.Infrastructure.WebAPI;
using ArticlesApp.WebAPI.NotificationsService;
using ArticlesApp.WebAPI.SignalRService.Articles;
using ArticlesApp.WebAPI.SignalRService.Notifications;
using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prometheus;



namespace ArticlesApp.WebAPI;



public class Startup
{
    private AppConfiguration AppConfiguration { get; }
    private AppSecrets AppSecrets { get; }
    private IConfiguration HostConfiguration { get; }
    private IWebHostEnvironment HostEnv { get; }

    public Startup(
        IConfiguration hostConfig,
        IWebHostEnvironment hostEnv
    )
    {
        HostConfiguration = hostConfig;
        HostEnv = hostEnv;

        AppSecrets = new AppSecretsBuilder(HostConfiguration)
            .Build()
            .Validate();

        AppConfiguration = AppConfigurationBuilder.Build(
            hostEnv,
            hostConfig,
            AppSecrets
        ).Validate();
    }



    public void ConfigureServices(IServiceCollection services)
    {
        // App configuration:

        services.AddSingleton(AppSecrets);
        services.AddSingleton(AppConfiguration);

        // Event Brokers:

        services.AddSingleton<INotificationsEventBroker, NotificationsEventBroker_LocalCaller>();
        services.AddSingleton<IArticlesEventBroker, ArticlesEventBroker_LocalCaller>();

        // Frontend loggers:

        services.AddSingleton<AppFrontend_LogsSingleton>();
        services.AddSingleton<AppFrontend_AnalyticsLogger>();

        // Backend analytics logger:

        services.AddSingleton<AnalyticsLogger>();

        // Database:

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(AppSecrets.DbConnectionString)
        );

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IArticlesRepository, ArticlesRepository_SqlServer>();
        services.AddScoped<INotificationsRepository, NotificationsRepository_SqlServer>();
        services.AddScoped<IUserRepository, UserRepository_SqlServer>();

        services.AddScoped<ArticlesOrchestrator>();
        services.AddScoped<NotificationsOrchestrator>();
        services.AddScoped<UsersOrchestrator>();

        services.AddScoped<DbInitializer>();
        services.AddScoped<SoftDeletionReaper>();

        services.AddScoped<UserManagerHelper>();

        // Automapper:

        MapperConfiguration mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new WebAPI._Helpers.AutomapperProfile());
            mc.AddProfile(new Database.SqlServer.Converters._Helpers.AutomapperProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<ArticlesConverter_WebAPI>();
        services.AddSingleton<NotificationsConverter_WebAPI>();
        services.AddSingleton<ArticleConverter_SqlServer>();
        services.AddSingleton<NotificationConverter_SqlServer>();

        // Identity:

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(opt =>
            {
                opt.ApplicationCookie.Configure(options =>
                {
                    string identityUIDefaultAreaName = "Identity";

                    options.LoginPath = $"/{identityUIDefaultAreaName}/Account/Login";
                    options.LogoutPath = $"/{identityUIDefaultAreaName}/Account/Logout";
                    options.AccessDeniedPath = 
                        $"/{identityUIDefaultAreaName}/Account/AccessDenied";
                });
            });

        services.AddIdentityCore<ApplicationUser_DB>(options =>
            {
                options.Stores.MaxLengthForKeys = 128;

                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.@-_ ";

                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddRoles<ApplicationUserRole_DB>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer(options =>
            {
                options.IssuerUri = "ArticlesApp";
            })
            .AddApiAuthorization<ApplicationUser_DB, ApplicationDbContext>(options =>
            {
                options.IdentityResources.Add(
                    new IdentityResource(
                        "roles",
                        new[] {
                            ClaimTypes.Role
                        }
                    )
                );

                SpaClientConfiguration spaClientConfiguration = 
                    AppConfiguration.IdentityServer.SpaClient;

                Client clientOptions = options.Clients.AddIdentityServerSPA(
                    spaClientConfiguration.ClientId,
                    configure => { }
                );

                clientOptions.AccessTokenLifetime = (int)TimeSpan
                    .FromMinutes(spaClientConfiguration.AccessTokenLifetimeMinutes)
                    .TotalSeconds;
                clientOptions.RedirectUris = spaClientConfiguration.RedirectUris;
                clientOptions.PostLogoutRedirectUris = 
                    spaClientConfiguration.PostLogoutRedirectUris;

                clientOptions.AllowedScopes.Add("roles");

                // Configure the refresh tokens:

                clientOptions.AllowOfflineAccess = true;
                clientOptions.AllowedScopes.Add("offline_access");
                clientOptions.RefreshTokenUsage = TokenUsage.OneTimeOnly;
            });

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddTransient<IProfileService, ProfileService>();

        services.AddAuthentication()
            .AddIdentityServerJwt()
            .AddGitHub(options =>
            {
                options.ClientId = AppSecrets.GitHubCredentials.ClientId;
                options.ClientSecret = AppSecrets.GitHubCredentials.ClientSecret;
                options.Scope.Add("user:email");
            })
            .AddGoogle(options =>
            {
                options.ClientId = AppSecrets.GoogleCredentials.ClientId;
                options.ClientSecret = AppSecrets.GoogleCredentials.ClientSecret;
                options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
            });

        services
            .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services.AddScoped<AccountMergeManager>();

        services
            .AddScoped<UserManager<ApplicationUser_DB>, UserManagerOverride<ApplicationUser_DB>>();

        // RazorPages:

        services.AddConnections();
        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddSignalR()
            .AddStackExchangeRedis(
                AppSecrets.SignalR_Redis_ConnectionString,
                options =>
                {
                    options.Configuration.ChannelPrefix = "ArticlesApp";
                    options.Configuration.AbortOnConnectFail = false;
                }
            );

        // SPA:

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "AppFrontend/build";
            // In production, the React files will be served from this directory.
        });

        // Event Subscription:

        services.AddSingleton<NotificationsHubSubscriber>();
        services.AddSingleton<NotificationsOrchestratorSubscriber>();
        services.AddSingleton<MyArticlesHubSubscriber>();
        services.AddSingleton<ModeratorArticlesHubSubscriber>();

        services.AddSingleton<NotificationSubscriptions>();
        services.AddSingleton<ArticleSubscriptions>();

        // Authorization Policies:

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "ModeratorOnly",
                policy => policy.RequireClaim(ClaimTypes.Role, "Moderator")
            );
        });
    }



    public void Configure(IApplicationBuilder app)
    {
        app.UseResponseInfoAnalytics();

        if (HostEnv.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            // The default HSTS value is 30 days.
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseRouting();
        app.UseHttpMetrics();
        app.Use_LogSessionId();

        // Identity:

        app.UseAuthentication();
        app.UseUserSoftDeleted();
        app.UseIdentityServer();
        app.UseAuthorization();

        // WebAPI, RazorPages and SPA:

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapHub<NotificationsHub>("/api/notificationsHub");
            endpoints.MapHub<MyArticlesHub>("/api/myArticlesHub");
            endpoints.MapHub<ModeratorArticlesHub>("/api/moderatorArticlesHub");
            endpoints.MapMetrics();
        });

        app.UseWebApiPageNotFound();
        app.UseIdentityPageNotFound();

        app.UseSpa(spaOptions =>
        {
            spaOptions.Options.SourcePath = "AppFrontend";
            spaOptions.ConfigureIfDevelopmentEnv(HostEnv);
        });

        Console.WriteLine("Web-server is ready." + Environment.NewLine);
    }
}