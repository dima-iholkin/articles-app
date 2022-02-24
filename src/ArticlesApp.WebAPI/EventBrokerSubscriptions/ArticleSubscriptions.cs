using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;
using ArticlesApp.Core.Orchestrators.OrchestratorSubscribers;
using ArticlesApp.WebAPI.SignalRService.Articles;



namespace ArticlesApp.WebAPI.EventBrokerSubscriptions;



public class ArticleSubscriptions
{
    private readonly IArticlesEventBroker _articlesEventBroker;
    private readonly ModeratorArticlesHubSubscriber _moderatorArticlesHubSender;
    private readonly MyArticlesHubSubscriber _myArticlesHubSender;
    private readonly NotificationsOrchestratorSubscriber _notificationsOrchestratorSubscriber;

    public ArticleSubscriptions(
        IArticlesEventBroker articlesEventBroker,
        ModeratorArticlesHubSubscriber moderatorArticlesHubSender,
        MyArticlesHubSubscriber articlesHubSender,
        NotificationsOrchestratorSubscriber notificationsOrchestratorSubscriber
    )
    {
        _articlesEventBroker = articlesEventBroker;
        _moderatorArticlesHubSender = moderatorArticlesHubSender;
        _myArticlesHubSender = articlesHubSender;
        _notificationsOrchestratorSubscriber = notificationsOrchestratorSubscriber;
    }



    public void RegisterSubscriptions()
    {
        Register_ArticleCreated();
        Register_ArticleModeratorDecision();
        Register_ArticleSoftDeleted();
    }



    private void Register_ArticleCreated()
    {
        ArticleCreatedEvent _event = _articlesEventBroker.ArticleCreatedEvent;

        _event.Handlers += _notificationsOrchestratorSubscriber.OnArticleCreated;
        _event.Handlers += _myArticlesHubSender.OnArticleCreated;
        _event.Handlers += _moderatorArticlesHubSender.OnArticleCreated;
    }



    private void Register_ArticleModeratorDecision()
    {
        ArticleModeratorDecisionEvent _event = _articlesEventBroker.ArticleModeratorDecisionEvent;

        _event.Handlers += _notificationsOrchestratorSubscriber.OnArticleModeratorDecision;
        _event.Handlers += _myArticlesHubSender.OnArticleModeratorDecision;
        _event.Handlers += _moderatorArticlesHubSender.OnArticleModeratorDecision;
    }



    private void Register_ArticleSoftDeleted()
    {
        ArticleSoftDeletedEvent _event = _articlesEventBroker.ArticleSoftDeletedEvent;

        _event.Handlers += _myArticlesHubSender.OnArticleSoftDeleted;
        _event.Handlers += _moderatorArticlesHubSender.OnArticleSoftDeleted;
    }
}