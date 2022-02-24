using ArticlesApp.Core.Entities.Notification;



namespace ArticlesApp.Core.Orchestrators.Models.Articles;



public class NotificationToCreate_Validated
{
    public NotificationToCreate_Validated(
        string message,
        string reciever_UserId
    )
    {
        Message = message;
        Reciever_UserId = reciever_UserId;
    }

    public string Message { get; set; }
    public string Reciever_UserId { get; set; }

    public NotificationTypesEnum? NofiticationType_TypeId { get; set; }
    public int? ReferencedArticle_ArticleId { get; set; }
}