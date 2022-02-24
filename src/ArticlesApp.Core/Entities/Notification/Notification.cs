namespace ArticlesApp.Core.Entities.Notification;



public class Notification : INotification
{
    public Notification(
        int id,
        string message,
        string reciever_UserId,
        DateTime createdAt_DateUtc
    )
    {
        Id = id;
        Message = message;
        Reciever_UserId = reciever_UserId;
        CreatedAt_DateUtc = createdAt_DateUtc;
    }

    public int Id { get; set; }

    public string Message { get; set; }

    public string Reciever_UserId { get; set; }

    public DateTime CreatedAt_DateUtc { get; set; }

    public DateTime? ReadAt_DateUtc { get; set; }

    public NotificationTypesEnum? NotificationType_TypeId { get; set; }

    public int? ReferencedArticle_ArticleId { get; set; }
}