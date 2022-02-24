using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;



public class ArticleSoftDeletedEvent : IEvent<ArticleArgs>
{
    private readonly ILogger<ArticleSoftDeletedEvent> _logger;

    public ArticleSoftDeletedEvent(ILogger<ArticleSoftDeletedEvent> logger)
    {
        _logger = logger;
    }


    public event EventHandler<ArticleArgs>? Handlers;



    public void Publish(object sender, ArticleArgs articleArgs)
    {
        if (Handlers == null)
        {
            _logger.LogWarning("The invoked handlers collection was empty.");
            return;
        }

        Handlers(sender, articleArgs);
    }
}