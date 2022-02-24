using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;



namespace ArticlesApp.WebAPI.SignalRService.Articles;



[Authorize(Policy = "ModeratorOnly")]
public class ModeratorArticlesHub : Hub
{ }