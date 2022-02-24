using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;



public class ArticleModeratorDecisionEvent : IEvent<ArticleArgs>
{
    private readonly ILogger<ArticleModeratorDecisionEvent> _logger;

    public ArticleModeratorDecisionEvent(ILogger<ArticleModeratorDecisionEvent> logger)
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