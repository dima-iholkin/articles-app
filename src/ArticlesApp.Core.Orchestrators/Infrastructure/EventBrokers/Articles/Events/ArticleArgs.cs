using ArticlesApp.Core.Entities.Article;



namespace ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;



public class ArticleArgs : EventArgs
{
    public ArticleArgs(Article article)
    {
        Article = article;
    }

    public Article Article { get; }
}