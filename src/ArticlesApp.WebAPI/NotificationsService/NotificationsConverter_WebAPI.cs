using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.WebAPI.NotificationsService.Models;
using AutoMapper;



namespace ArticlesApp.WebAPI.NotificationsService;



public class NotificationsConverter_WebAPI
{
    private readonly IMapper _mapper;

    public NotificationsConverter_WebAPI(IMapper mapper)
    {
        _mapper = mapper;
    }



    public Notification ToEntity<T>(Notification_WebAPI fromEntity)
        where T : Notification
    {
        return _mapper.Map<Notification_WebAPI, Notification>(fromEntity);
    }



    public Notification_WebAPI ToEntity<T>(Notification fromEntity)
        where T : Notification_WebAPI
    {
        return _mapper.Map<Notification, Notification_WebAPI>(fromEntity);
    }



    public IEnumerable<Notification_WebAPI> ToEntities<T>(IEnumerable<Notification> fromEntities)
        where T : Notification_WebAPI
    {
        IEnumerable<Notification_WebAPI> toEntities = fromEntities
            .Select((notification) => ToEntity<Notification_WebAPI>(notification));

        return toEntities;
    }



    public IEnumerable<Notification> ToEntities<T>(IEnumerable<Notification_WebAPI> fromEntities)
        where T : Notification
    {
        IEnumerable<Notification> toEntities = fromEntities
            .Select((notification) => ToEntity<Notification>(notification));

        return toEntities;
    }
}