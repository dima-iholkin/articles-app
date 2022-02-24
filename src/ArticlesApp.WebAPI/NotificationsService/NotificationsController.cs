using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.WebAPI.Helpers;
using ArticlesApp.WebAPI.NotificationsService.Models;
using ArticlesApp.WebAPI.SignalRService.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;



namespace ArticlesApp.WebAPI.NotificationsService;



[Route("api/notifications")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly NotificationsOrchestrator _notificationsOrchestrator;
    private readonly NotificationsConverter_WebAPI _converter;
    private readonly IHubContext<NotificationsHub> _notificationsHub;

    public NotificationsController(
        NotificationsOrchestrator notificationsOrchestrator,
        NotificationsConverter_WebAPI converter,
        IHubContext<NotificationsHub> notificationsHub
    )
    {
        _notificationsOrchestrator = notificationsOrchestrator;
        _converter = converter;
        _notificationsHub = notificationsHub;
    }



    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Notification_WebAPI[]>> GetAllNotificationsForUser()
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        Notification[] notifs = await _notificationsOrchestrator.GetAllNotificationsForUserAsync(userId);
        Notification_WebAPI[] notifs_WebAPI = _converter.ToEntities<Notification_WebAPI>(notifs)
            .ToArray();

        return Ok(notifs_WebAPI);
    }
    // GET: api/notifications



    [HttpGet("new/{newestFetchedNotifId}")]
    [Authorize]
    public async Task<ActionResult<Notification_WebAPI[]>> GetNewNotificationsByNewestFetchedAsync(int newestFetchedNotifId)
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        Notification[] notifs = await _notificationsOrchestrator.GetNewNotificationsByNewestFetchedAsync(
            userId,
            newestFetchedNotifId
        );
        Notification_WebAPI[] notifs_WebAPI = _converter.ToEntities<Notification_WebAPI>(notifs)
            .ToArray();

        return Ok(notifs_WebAPI);
    }
    // GET: api/notifications/new/5



    [HttpPut("{notifId}")]
    [Authorize]
    public async Task<ActionResult<Notification_WebAPI>> MarkNotificationAsRead(int notifId)
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            Notification notif = await _notificationsOrchestrator.MarkNotificationAsReadAsync(
                userId,
                notifId
            );
            Notification_WebAPI notif_WebAPI = _converter.ToEntity<Notification_WebAPI>(notif);

            return Ok(notif_WebAPI);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedUserException)
        {
            return Forbid();
        }
    }
    // PUT: api/notifications/5



    [Authorize]
    [HttpPost("_testhub")]
    public async Task<IActionResult> TestNotificationsHub()
    {
        string? userId = User.GetUserId();

        NotificationPayload payload = new NotificationPayload(
            new Notification_WebAPI(
                0,
                "Hello from test",
                userId,
                DateTime.UtcNow
            )
        );

        await _notificationsHub.Clients.User(userId).SendAsync(
            "NotificationCreated",
            payload
        );

        //_notificationsHub.Clients.

        //await _notificationsHub.NotificationCreated(
        //    userId,
        //    notificationPayload
        //);

        return Ok();
        // POST: api/notifications/_testhub
    }
}