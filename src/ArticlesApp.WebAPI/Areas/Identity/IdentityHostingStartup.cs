using ArticlesApp.WebAPI.Areas.Identity;
using Microsoft.AspNetCore.Hosting;



[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace ArticlesApp.WebAPI.Areas.Identity;



public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { });
    }
}