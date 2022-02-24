using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;



public class ArticleCreatedEvent : IEvent<ArticleArgs>
{
    private readonly ILogger<ArticleCreatedEvent> _logger;

    public ArticleCreatedEvent(ILogger<ArticleCreatedEvent> logger)
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