namespace ArticlesApp.WebAPI.AppFrontend_AnalyticsService.Events;



public class AppFrontend_RouteChange
{
    public string? ClientLoggingId { get; set; }
    public string? Route { get; set; }
}