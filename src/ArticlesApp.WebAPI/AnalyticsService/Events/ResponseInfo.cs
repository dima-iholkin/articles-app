namespace ArticlesApp.WebAPI.AnalyticsService.Events;



public class ResponseInfo
{
    public string? ClientLoggingId { get; set; }
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? QueryString { get; set; }
    public string? Scheme { get; set; }
    public string? Host { get; set; }
    public int ResponseCode { get; set; }
}