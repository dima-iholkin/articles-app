using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Database.SqlServer.Models;
using AutoMapper;



namespace ArticlesApp.Database.SqlServer.Converters;



public class NotificationConverter_SqlServer
{
    private readonly IMapper _mapper;

    public NotificationConverter_SqlServer(IMapper mapper)
    {
        _mapper = mapper;
    }



    public Notification ToEntity<T>(Notification_SqlServer fromEntity)
        where T : Notification
    {
        return _mapper.Map<Notification_SqlServer, Notification>(fromEntity);
    }



    public Notification_SqlServer ToEntity<T>(Notification fromEntity)
        where T : Notification_SqlServer
    {
        return _mapper.Map<Notification, Notification_SqlServer>(fromEntity);
    }



    public Notification_SqlServer ToEntity<T>(NotificationToCreate fromEntity)
        where T : Notification_SqlServer
    {
        Notification_SqlServer notification_SqlServer = new Notification_SqlServer
        (
            id: 0,
            message: fromEntity.Message,
            reciever_UserId: fromEntity.Reciever_UserId,
            createdAt_DateUtc: fromEntity.CreatedAt_DateUtc
        )
        {
            NotificationType_TypeId = fromEntity.NotificationType_TypeId,
            ReferencedArticle_ArticleId = fromEntity.ReferencedArticle_ArticleId
        };

        return notification_SqlServer;
    }



    public IEnumerable<Notification_SqlServer> ToEntities<T>(IEnumerable<Notification> fromEntities)
        where T : Notification_SqlServer
    {
        IEnumerable<Notification_SqlServer> toEntities = fromEntities
            .Select((notification) => this.ToEntity<Notification_SqlServer>(notification));
        return toEntities;
    }



    public IEnumerable<Notification> ToEntities<T>(IEnumerable<Notification_SqlServer> fromEntities)
        where T : Notification
    {
        IEnumerable<Notification> toEntities = fromEntities
            .Select((notification) => ToEntity<Notification>(notification));
        return toEntities;
    }
}