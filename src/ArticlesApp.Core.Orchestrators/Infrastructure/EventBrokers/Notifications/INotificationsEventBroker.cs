using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications;



public interface INotificationsEventBroker
{
    public NotificationCreatedEvent NotificationCreatedEvent { get; }

    public NotificationReadEvent NotificationReadEvent { get; }
}