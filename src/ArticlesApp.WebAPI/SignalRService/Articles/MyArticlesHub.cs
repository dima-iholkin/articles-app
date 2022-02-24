using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;



namespace ArticlesApp.WebAPI.SignalRService.Articles;



[Authorize]
public class MyArticlesHub : Hub
{ }