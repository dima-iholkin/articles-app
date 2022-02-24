using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Database.SqlServer.Models;
using AutoMapper;



namespace ArticlesApp.Database.SqlServer.Converters._Helpers;



public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<Article_SqlServer, Article>().ReverseMap();
        CreateMap<Notification_SqlServer, Notification>().ReverseMap();
    }
}