using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Models.Articles;



namespace ArticlesApp.Core.Orchestrators.Validators;



public static class NotificationValidator
{
    public static NotificationToCreate_Validated Validate(this NotificationToCreate_Submitted notification)
    {
        if (
            String.IsNullOrEmpty(notification.Message)
            || String.IsNullOrEmpty(notification.Reciever_UserId)
        )
        {
            throw new ValidationException(
                $"The required fields " +
                    $"{nameof(notification.Message)} and {nameof(notification.Reciever_UserId)} " +
                    $"should not be empty."
            );
        }

        NotificationToCreate_Validated notificationValidated = new NotificationToCreate_Validated
        (
            message: notification.Message,
            reciever_UserId: notification.Reciever_UserId
        )
        {
            NofiticationType_TypeId = notification.NotificationType_TypeId,
            ReferencedArticle_ArticleId = notification.ReferencedArticle_ArticleId
        };

        return notificationValidated;
    }
}