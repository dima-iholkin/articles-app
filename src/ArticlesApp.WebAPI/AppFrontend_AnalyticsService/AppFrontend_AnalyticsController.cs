using ArticlesApp.WebAPI.AppFrontend_AnalyticsService.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace ArticlesApp.WebAPI.AppFrontend_AnalyticsService;



[Route("api/analytics")]
[ApiController]
public class AppFrontend_AnalyticsController : ControllerBase
{
    private readonly AppFrontend_AnalyticsLogger _logger;

    public AppFrontend_AnalyticsController(AppFrontend_AnalyticsLogger logger) : base()
    {
        _logger = logger;
    }



    [HttpPost]
    public ActionResult PostLogs([FromBody] RouteChangeItem[] logs)
    {
        HttpContext.Request.Cookies.TryGetValue(
            "ClientLoggingId",
            out string? clientLoggingId
        );

        foreach (RouteChangeItem routeChangeItem in logs.Reverse())
        {
            AppFrontend_RouteChange routeChangeLog = new AppFrontend_RouteChange
            {
                ClientLoggingId = clientLoggingId,
                Route = routeChangeItem.RouteChange
            };

            _logger.Log_RouteChange(routeChangeLog);
        }
        // Recurse the logs array from the tail,
        // Because this is the way the AppFrontend logger sends the data.

        return Ok();
    }
    // POST: api/analytics



    public struct RouteChangeItem
    {
        public string RouteChange { get; set; }
    }
}