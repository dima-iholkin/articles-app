using ArticlesApp.WebAPI.NotificationsService.Models;



namespace ArticlesApp.WebAPI.SignalRService.Notifications;



public class NotificationPayload
{
    public NotificationPayload(Notification_WebAPI notification)
    {
        Notification = notification;
    }

    public Notification_WebAPI Notification { get; }
}