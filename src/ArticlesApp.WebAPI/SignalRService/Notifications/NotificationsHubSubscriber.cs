using ArticlesApp._Utilities._Helpers;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;
using ArticlesApp.WebAPI.NotificationsService;
using ArticlesApp.WebAPI.NotificationsService.Models;
using Microsoft.AspNetCore.SignalR;



namespace ArticlesApp.WebAPI.SignalRService.Notifications;



public class NotificationsHubSubscriber
{
    private readonly ILogger<NotificationsHubSubscriber> _logger;
    private readonly NotificationsConverter_WebAPI _notificationsConverter;
    private readonly IHubContext<NotificationsHub> _notificationsHubContext;

    public NotificationsHubSubscriber(
        ILogger<NotificationsHubSubscriber> logger,
        NotificationsConverter_WebAPI notificationsConverter,
        IHubContext<NotificationsHub> notificationsHub
    )
    {
        _logger = logger;
        _notificationsConverter = notificationsConverter;
        _notificationsHubContext = notificationsHub;
    }



    public void OnNotificationCreated(
        object? sender,
        NotificationArgs notificationArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnNotificationCreated(sender, notificationArgs),
            _logger
        );
    }



    public void OnNotificationRead(
        object? sender,
        NotificationArgs notificationArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnNotificationRead(sender, notificationArgs),
            _logger
        );
    }



    private async Task _OnNotificationCreated(
            object? sender,
        NotificationArgs notificationArgs
    )
    {
        Notification_WebAPI notification_WebAPI = _notificationsConverter
            .ToEntity<Notification_WebAPI>(notificationArgs.Notification);

        string userId = notificationArgs.Notification.Reciever_UserId;
        NotificationPayload payload = new NotificationPayload(notification_WebAPI);

        await _notificationsHubContext.Clients.User(userId).SendAsync(
            "NotificationCreated",
            payload
        );
    }



    private async Task _OnNotificationRead(
            object? sender,
        NotificationArgs notificationArgs
    )
    {
        Notification_WebAPI notification_WebAPI = _notificationsConverter
            .ToEntity<Notification_WebAPI>(notificationArgs.Notification);

        string userId = notificationArgs.Notification.Reciever_UserId;
        NotificationPayload payload = new NotificationPayload(notification_WebAPI);

        await _notificationsHubContext.Clients.User(userId).SendAsync(
            "NotificationRead",
            payload
        );
    }
}