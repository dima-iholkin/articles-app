using ArticlesApp.Core.Entities.Notification;



namespace ArticlesApp.Core.Orchestrators.Models.Articles;



public class NotificationToCreate_Submitted
{
    public string? Message { get; set; }
    public string? Reciever_UserId { get; set; }

    public NotificationTypesEnum? NotificationType_TypeId { get; set; }
    public int? ReferencedArticle_ArticleId { get; set; }
}