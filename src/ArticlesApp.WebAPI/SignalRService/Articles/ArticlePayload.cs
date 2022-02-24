using ArticlesApp.WebAPI.Models;



namespace ArticlesApp.WebAPI.SignalRService.Articles;



public class ArticlePayload
{
    public ArticlePayload(Article_WebAPI article)
    {
        Article = article;
    }

    public Article_WebAPI Article { get; }
}