using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Database.SqlServer.Converters;
using ArticlesApp.Database.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;



namespace ArticlesApp.Database.SqlServer.Repositories;



public class NotificationsRepository_SqlServer : INotificationsRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly NotificationConverter_SqlServer _converter;
    //private readonly ILogger _logger;

    public NotificationsRepository_SqlServer(
        ApplicationDbContext dbContext,
        NotificationConverter_SqlServer converter
        //ILogger logger
    )
    {
        _dbContext = dbContext;
        _converter = converter;
        //_logger = logger;
    }



    public async Task<Notification[]> GetAllNotificationsAsync(string userId)
    {
        Notification_SqlServer[] notifs_SqlServer = await _dbContext.Notifications
            .Where(notif => notif.Reciever_UserId == userId)
            .ToArrayAsync();

        IEnumerable<Notification> notifs = _converter.ToEntities<Notification>(notifs_SqlServer);
        return notifs.ToArray();
    }



    public async Task<Notification> GetNotificationByIdAsync(
        string userId,
        int notificationId
    )
    {
        Notification_SqlServer notif_SqlServer = await _dbContext.Notifications.FindAsync(
            userId,
            notificationId
        ) ?? throw new EntityNotFoundException();

        Notification notif = _converter.ToEntity<Notification>(notif_SqlServer);
        return notif;
    }



    public async Task<Notification[]> GetNewNotificationsByNewestFetchedAsync(
        string userId,
        int newestFetchedNotificationId
    )
    {
        Notification_SqlServer[] notifs_SqlServer = await _dbContext.Notifications
            .Where(notif => notif.Reciever_UserId == userId)
            .Where(notif => notif.Id > newestFetchedNotificationId)
            .OrderBy(notif => notif.Id)
            .ToArrayAsync();

        IEnumerable<Notification> notifs = _converter.ToEntities<Notification>(notifs_SqlServer);
        return notifs.ToArray();
    }



    public async Task<Notification> MarkNotificationAsReadAsync(
        string userId,
        DateTime readAt_DateUtc,
        int notificationId
    )
    {
        Notification_SqlServer notif_SqlServer = await _dbContext.Notifications.FindAsync(
            userId,
            notificationId
        ) ?? throw new EntityNotFoundException();

        _dbContext.Entry(notif_SqlServer)
            .Property(n => n.ReadAt_DateUtc)
            .CurrentValue = readAt_DateUtc;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        Notification notif = _converter.ToEntity<Notification>(notif_SqlServer);
        return notif;
    }



    public async Task<Notification> CreateNotificationAsync(NotificationToCreate notif)
    {
        Notification_SqlServer notif_SqlServer = _converter.ToEntity<Notification_SqlServer>(notif);

        _dbContext.Notifications.Add(notif_SqlServer);
        await _dbContext.SaveChangesAsync();

        Notification notifSaved = _converter.ToEntity<Notification>(notif_SqlServer);
        return notifSaved;
    }
}