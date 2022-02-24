using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;



public class NotificationReadEvent : IEvent<NotificationArgs>
{
    private readonly ILogger<NotificationReadEvent> _logger;

    public NotificationReadEvent(ILogger<NotificationReadEvent> logger)
    {
        _logger = logger;
    }



    public event EventHandler<NotificationArgs>? Handlers;



    public void Publish(object sender, NotificationArgs notificationArgs)
    {
        if (Handlers == null)
        {
            _logger.LogWarning("The invoked handlers collection was empty.");
            return;
        }

        Handlers(sender, notificationArgs);
    }
}