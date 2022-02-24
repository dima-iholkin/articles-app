using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.WebAPI.Models;
using ArticlesApp.WebAPI.NotificationsService.Models;
using AutoMapper;



namespace ArticlesApp.WebAPI._Helpers;



public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<Article, Article_WebAPI>().ReverseMap();
        CreateMap<Notification, Notification_WebAPI>().ReverseMap();
    }
}