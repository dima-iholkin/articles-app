using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications;
using ArticlesApp.WebAPI.SignalRService.Notifications;



namespace ArticlesApp.WebAPI.EventBrokerSubscriptions;



public class NotificationSubscriptions
{
    private readonly INotificationsEventBroker _notificationsEventBroker;
    private readonly NotificationsHubSubscriber _notificationsHubSender;

    public NotificationSubscriptions(
        INotificationsEventBroker notificationsEventBroker,
        NotificationsHubSubscriber notificationsHubSender
    )
    {
        _notificationsEventBroker = notificationsEventBroker;
        _notificationsHubSender = notificationsHubSender;
    }



    public void RegisterSubscriptions()
    {
        Register_NotificationCreated();
        Register_NotificationRead();
    }



    private void Register_NotificationCreated()
    {
        _notificationsEventBroker.NotificationCreatedEvent.Handlers += _notificationsHubSender.OnNotificationCreated;
    }



    private void Register_NotificationRead()
    {
        _notificationsEventBroker.NotificationReadEvent.Handlers += _notificationsHubSender.OnNotificationRead;
    }
}