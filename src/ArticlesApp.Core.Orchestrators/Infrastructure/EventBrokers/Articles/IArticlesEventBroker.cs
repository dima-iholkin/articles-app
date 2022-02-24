using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles;



public interface IArticlesEventBroker
{
    public ArticleCreatedEvent ArticleCreatedEvent { get; }

    public ArticleModeratorDecisionEvent ArticleModeratorDecisionEvent { get; }

    public ArticleSoftDeletedEvent ArticleSoftDeletedEvent { get; }
}