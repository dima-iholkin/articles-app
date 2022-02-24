using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;
using Microsoft.Extensions.Logging;



namespace ArticlesApp.EventBroker.LocalCaller;



public class NotificationsEventBroker_LocalCaller : INotificationsEventBroker
{
    public NotificationsEventBroker_LocalCaller(
        ILogger<NotificationCreatedEvent> logger_NotificationCreatedEvent,
        ILogger<NotificationReadEvent> logger_NotificationReadEvent
    )
    {
        NotificationCreatedEvent = new NotificationCreatedEvent(logger_NotificationCreatedEvent);
        NotificationReadEvent = new NotificationReadEvent(logger_NotificationReadEvent);
    }



    public NotificationCreatedEvent NotificationCreatedEvent { get; }
    public NotificationReadEvent NotificationReadEvent { get; }
}