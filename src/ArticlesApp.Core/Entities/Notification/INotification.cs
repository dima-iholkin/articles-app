namespace ArticlesApp.Core.Entities.Notification;



public interface INotification
{
    public int Id { get; set; }

    public string Message { get; set; }

    public string Reciever_UserId { get; set; }

    public DateTime CreatedAt_DateUtc { get; set; }

    public DateTime? ReadAt_DateUtc { get; set; }

    public NotificationTypesEnum? NotificationType_TypeId { get; set; }

    public int? ReferencedArticle_ArticleId { get; set; }
}