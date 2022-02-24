using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Database.SqlServer.Models;
using AutoMapper;



namespace ArticlesApp.Database.SqlServer.Converters;



public class ArticleConverter_SqlServer
{
    private readonly IMapper _mapper;

    public ArticleConverter_SqlServer(IMapper mapper)
    {
        _mapper = mapper;
    }



    public Article ToEntity<T>(Article_SqlServer fromEntity)
        where T : Article
    {
        return _mapper.Map<Article_SqlServer, Article>(fromEntity);
    }



    public Article_SqlServer ToEntity<T>(Article fromEntity)
        where T : Article_SqlServer
    {
        return _mapper.Map<Article, Article_SqlServer>(fromEntity);
    }



    public Article_SqlServer ToEntity<T>(ArticleToCreate fromEntity)
        where T : Article_SqlServer
    {
        Article_SqlServer article_SqlServer = new Article_SqlServer
        (
            id: 0,
            title: fromEntity.Title,
            text: fromEntity.Text,
            submittedAt_DateUtc: fromEntity.SubmittedAt_DateUtc,
            authorId: fromEntity.AuthorId,
            articleStateId: fromEntity.ArticleStateId,
            versionId: 0
        );

        return article_SqlServer;
    }



    public IEnumerable<Article_SqlServer> ToEntities<T>(IEnumerable<Article> fromEntities)
        where T : Article_SqlServer
    {
        IEnumerable<Article_SqlServer> toEntities = fromEntities
            .Select((article) => ToEntity<Article_SqlServer>(article));
        return toEntities;
    }



    public IEnumerable<Article_SqlServer> ToEntities<T>(IEnumerable<ArticleToCreate> fromEntities)
        where T : Article_SqlServer
    {
        IEnumerable<Article_SqlServer> toEntities = fromEntities
            .Select((article) => ToEntity<Article_SqlServer>(article));
        return toEntities;
    }



    public IEnumerable<Article> ToEntities<T>(IEnumerable<Article_SqlServer> fromEntities)
        where T : Article
    {
        IEnumerable<Article> toEntities = fromEntities
            .Select((article) => ToEntity<Article>(article));
        return toEntities;
    }
}