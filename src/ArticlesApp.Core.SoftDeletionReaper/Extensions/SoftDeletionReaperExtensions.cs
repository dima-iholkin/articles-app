using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



namespace ArticlesApp.Core.SoftDeletionReaper;



public static class SoftDeletionReaperExtensions
{
    public static void RunReaperMode(
        IHost host,
        int softDeletionPeriod_Days
    )
    {
        using IServiceScope scope = host.Services.CreateScope();
        SoftDeletionReaper softDeletionReaper = scope.ServiceProvider.GetRequiredService<SoftDeletionReaper>();

        softDeletionReaper.RunReaperAsync(
            DateTime.UtcNow,
            softDeletionPeriod_Days
        ).GetAwaiter().GetResult();
    }
}