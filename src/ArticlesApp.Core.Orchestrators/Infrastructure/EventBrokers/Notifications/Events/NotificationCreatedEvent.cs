using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;



public class NotificationCreatedEvent : IEvent<NotificationArgs>
{
    private readonly ILogger<NotificationCreatedEvent> _logger;

    public NotificationCreatedEvent(ILogger<NotificationCreatedEvent> logger)
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