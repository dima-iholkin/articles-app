//using ArticlesApp.Core.Entities.Article;
//using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;
//using ArticlesApp.WebAPI.ArticlesService;
//using ArticlesApp.WebAPI.Models;
//using Microsoft.AspNetCore.SignalR;



//namespace ArticlesApp.WebAPI.SignalRService.Articles;



//public class PublicArticlesHubSubscriber
//{
//    private readonly ArticlesConverter_WebAPI _articlesConverter;
//    private readonly IHubContext<PublicArticlesHub> _articlesHubContext;

//    public PublicArticlesHubSubscriber(
//        ArticlesConverter_WebAPI articlesConverter,
//        IHubContext<PublicArticlesHub> articlesHubContext
//    )
//    {
//        _articlesConverter = articlesConverter;
//        _articlesHubContext = articlesHubContext;
//    }



//    public async Task OnArticleModeratorDecision(
//        object? sender,
//        ArticleArgs articleArgs
//    )
//    {
//        Article article = articleArgs.Article;
//        if (article.ArticleStateId != ArticleStatesEnum.Approved)
//        {
//            return;
//        }

//        Article_WebAPI article_WebAPI = _articlesConverter.ToEntity<Article_WebAPI>(article);
//        ArticlePayload payload = new ArticlePayload(article_WebAPI);

//        await _articlesHubContext.Clients.All.SendAsync(
//            "ArticleBecamePublic",
//            payload
//        );
//    }



//    public async Task OnArticleSoftDeleted(
//        object? sender,
//        ArticleArgs articleArgs
//    )
//    {
//        Article article = articleArgs.Article;
//        if (article.ArticleStateId != ArticleStatesEnum.Approved)
//        {
//            return;
//        }

//        Article_WebAPI article_WebAPI = _articlesConverter.ToEntity<Article_WebAPI>(article);
//        ArticlePayload payload = new ArticlePayload(article_WebAPI);

//        await _articlesHubContext.Clients.All.SendAsync(
//            "PublicArticleSoftDeleted",
//            payload
//        );
//    }
//}