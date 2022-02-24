using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;
using Microsoft.Extensions.Logging;



namespace ArticlesApp.EventBroker.LocalCaller;



public class ArticlesEventBroker_LocalCaller : IArticlesEventBroker
{
    public ArticlesEventBroker_LocalCaller(
        ILogger<ArticleCreatedEvent> logger_ArticleCreatedEvent,
        ILogger<ArticleModeratorDecisionEvent> logger_ArticleModeratorDecisionEvent,
        ILogger<ArticleSoftDeletedEvent> logger_ArticleSoftDeletedEvent
    )
    {
        ArticleCreatedEvent = new ArticleCreatedEvent(logger_ArticleCreatedEvent);
        ArticleModeratorDecisionEvent = new ArticleModeratorDecisionEvent(logger_ArticleModeratorDecisionEvent);
        ArticleSoftDeletedEvent = new ArticleSoftDeletedEvent(logger_ArticleSoftDeletedEvent);
    }



    public ArticleCreatedEvent ArticleCreatedEvent { get; }
    public ArticleModeratorDecisionEvent ArticleModeratorDecisionEvent { get; }
    public ArticleSoftDeletedEvent ArticleSoftDeletedEvent { get; }
}