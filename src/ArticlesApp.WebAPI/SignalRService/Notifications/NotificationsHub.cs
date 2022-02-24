using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;



namespace ArticlesApp.WebAPI.SignalRService.Notifications;



[Authorize]
public class NotificationsHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}