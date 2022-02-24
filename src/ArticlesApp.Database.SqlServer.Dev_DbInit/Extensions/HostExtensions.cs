namespace ArticlesApp.Database.SqlServer.Dev_DbInitializer;



public static class HostExtensions
{
    public static void RunDbInitialization(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        DbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
        
        dbInitializer.InitializeDb();
    }
}