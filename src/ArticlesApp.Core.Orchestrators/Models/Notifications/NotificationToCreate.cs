using ArticlesApp.Core.Entities.Notification;



namespace ArticlesApp.Core.Orchestrators.Models.Articles;



public class NotificationToCreate
{
    public NotificationToCreate(
        string message,
        string reciever_UserId,
        DateTime createdAt_DateUtc
    )
    {
        Message = message;
        Reciever_UserId = reciever_UserId;
        CreatedAt_DateUtc = createdAt_DateUtc;
    }

    public string Message { get; set; }
    public string Reciever_UserId { get; set; }
    public DateTime CreatedAt_DateUtc { get; set; }

    public NotificationTypesEnum? NotificationType_TypeId { get; set; }
    public int? ReferencedArticle_ArticleId { get; set; }
}