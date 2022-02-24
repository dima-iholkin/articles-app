using ArticlesApp._Utilities._Helpers;
using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators.OrchestratorSubscribers;



public class NotificationsOrchestratorSubscriber
{
    private readonly ILogger<NotificationsOrchestratorSubscriber> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationsOrchestratorSubscriber(
        ILogger<NotificationsOrchestratorSubscriber> logger,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }



    public void OnArticleCreated(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnArticleCreated(sender, articleArgs),
            _logger
        );
    }



    public void OnArticleModeratorDecision(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        EventHelpers.FireAndForget(
            () => _OnArticleModeratorDecision(sender, articleArgs),
            _logger
        );
    }



    private async Task _OnArticleCreated(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        Article articleSaved = articleArgs.Article;

        NotificationToCreate_Submitted notificationSubmitted = new NotificationToCreate_Submitted()
        {
            Message = $"\"{articleSaved.Title}\" article was submitted. Waiting for a moderator's decision.",
            Reciever_UserId = articleSaved.AuthorId,
            NotificationType_TypeId = NotificationTypesEnum.ArticleStateChanged,
            ReferencedArticle_ArticleId = articleSaved.Id
        };

        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        NotificationsOrchestrator notificationsOrchestrator = scope.ServiceProvider
            .GetRequiredService<NotificationsOrchestrator>();
        await notificationsOrchestrator.TryCreateNotificationAsync(notificationSubmitted);
    }



    private async Task _OnArticleModeratorDecision(
        object? sender,
        ArticleArgs articleArgs
    )
    {
        Article articleSaved = articleArgs.Article;

        NotificationToCreate_Submitted notificationSubmitted = new NotificationToCreate_Submitted
        {
            Message = $"\"{articleSaved.Title}\" article was {articleSaved.ArticleStateId.ToString().ToLower()}.",
            Reciever_UserId = articleSaved.AuthorId,
            NotificationType_TypeId = NotificationTypesEnum.ArticleStateChanged,
            ReferencedArticle_ArticleId = articleSaved.Id
        };

        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        NotificationsOrchestrator notificationsOrchestrator = scope.ServiceProvider
            .GetRequiredService<NotificationsOrchestrator>();
        await notificationsOrchestrator.CreateNotificationAsync(notificationSubmitted);
    }
}