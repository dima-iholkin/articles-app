using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Notifications.Events;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Core.Orchestrators.Validators;
using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators;



public class NotificationsOrchestrator
{
    private readonly ILogger<NotificationsOrchestrator> _logger;
    private readonly INotificationsEventBroker _notifsEventBroker;
    private readonly INotificationsRepository _notifsRepository;

    public NotificationsOrchestrator(
        ILogger<NotificationsOrchestrator> logger,
        INotificationsEventBroker notifsEventBroker,
        INotificationsRepository notifsRepository
    )
    {
        _logger = logger;
        _notifsEventBroker = notifsEventBroker;
        _notifsRepository = notifsRepository;
    }



    public async Task<Notification> CreateNotificationAsync(NotificationToCreate_Submitted notificationSubmitted)
    {
        NotificationToCreate_Validated notificationValidated = notificationSubmitted.Validate();

        NotificationToCreate notification = new NotificationToCreate(
            message: notificationValidated.Message,
            reciever_UserId: notificationValidated.Reciever_UserId,
            createdAt_DateUtc: DateTime.UtcNow
        )
        {
            NotificationType_TypeId = notificationValidated.NofiticationType_TypeId,
            ReferencedArticle_ArticleId = notificationValidated.ReferencedArticle_ArticleId
        };

        Notification notifCreated = await _notifsRepository.CreateNotificationAsync(notification);

        _notifsEventBroker.NotificationCreatedEvent.Publish(
            this,
            new NotificationArgs(notifCreated)
        );

        return notifCreated;
        // TODO: The notification creation logic should be universal to any notification type,
        // not just this one. #753
    }



    public async Task<Notification[]> GetAllNotificationsForUserAsync(string userId)
    {
        return await _notifsRepository.GetAllNotificationsAsync(userId);
    }



    public async Task<Notification[]> GetNewNotificationsByNewestFetchedAsync(
        string userId,
        int newestFetchedNotificationId
    )
    {
        return await _notifsRepository.GetNewNotificationsByNewestFetchedAsync(
            userId,
            newestFetchedNotificationId
        );
    }



    public async Task<Notification> MarkNotificationAsReadAsync(
        string userId,
        int notificationId
    )
    {
        Notification notif = await _notifsRepository.GetNotificationByIdAsync(
            userId,
            notificationId
        ) ?? throw new EntityNotFoundException();

        if (notif.Reciever_UserId != userId)
        {
            throw new UnauthorizedUserException();
        }

        if (notif.ReadAt_DateUtc != null)
        {
            _logger.LogWarning("Notification was read already.");

            return notif;
        }

        Notification notifSaved = await _notifsRepository.MarkNotificationAsReadAsync(
            userId,
            DateTime.UtcNow,
            notificationId
        ) ?? throw new EntityNotFoundException();

        _notifsEventBroker.NotificationReadEvent.Publish(
            this,
            new NotificationArgs(notifSaved)
        );

        return notifSaved;
    }



    public async Task<(bool, Notification?)> TryCreateNotificationAsync(NotificationToCreate_Submitted notificationSubmitted)
    {
        Notification notification;
        try
        {
            notification = await CreateNotificationAsync(notificationSubmitted);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "NotificationsOrchestrator threw an exception. Message: {Message}. Stack: {Stack}.",
                ex.Message,
                ex.StackTrace
            );
            return (false, null);
        }

        return (true, notification);
    }
}