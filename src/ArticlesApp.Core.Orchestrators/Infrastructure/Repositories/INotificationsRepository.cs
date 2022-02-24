using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Orchestrators.Models.Articles;



namespace ArticlesApp.Core.Orchestrators.Infrastructure;



public interface INotificationsRepository
{
    public Task<Notification[]> GetAllNotificationsAsync(string userId);

    public Task<Notification> GetNotificationByIdAsync(
        string userId,
        int notificationId
    );

    public Task<Notification[]> GetNewNotificationsByNewestFetchedAsync(
        string userId,
        int newestFetchedNotificationId
    );

    public Task<Notification> MarkNotificationAsReadAsync(
        string userId,
        DateTime readAt_DateUtc,
        int notificationId
    );

    public Task<Notification> CreateNotificationAsync(NotificationToCreate notif);
}