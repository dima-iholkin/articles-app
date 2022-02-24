using ArticlesApp.Core.Entities.Notification;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;



public class NotificationArgs : EventArgs
{
    public NotificationArgs(Notification notification)
    {
        Notification = notification;
    }

    public Notification Notification { get; }
}