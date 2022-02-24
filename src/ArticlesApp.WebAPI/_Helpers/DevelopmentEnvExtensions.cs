using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;



namespace ArticlesApp.WebAPI.Helpers;



public static class DevelopmentEnvExtensions
{
    public static void ConfigureIfDevelopmentEnv(
        this ISpaBuilder spaOptions,
        IHostEnvironment env
    )
    {
        if (env.IsDevelopment())
        {
            spaOptions.UseReactDevelopmentServer(npmScript: "start");
        }
    }
}