using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.PersonalData;
using ArticlesApp.Core.Entities.SoftDeletion;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Database.SqlServer._Helpers;
using ArticlesApp.Database.SqlServer.Converters;
using ArticlesApp.Database.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace ArticlesApp.Database.SqlServer.Repositories;



public class ArticlesRepository_SqlServer : IArticlesRepository
{
    private readonly ArticleConverter_SqlServer _converter;
    private readonly ApplicationDbContext _dbContext;

    public ArticlesRepository_SqlServer(
        ArticleConverter_SqlServer converter,
        ApplicationDbContext dbContext
    )
    {
        _converter = converter;
        _dbContext = dbContext;
    }



    public async Task<Article> AddArticleAsync(ArticleToCreate article)
    {
        Article_SqlServer article_SqlServer = _converter.ToEntity<Article_SqlServer>(article);

        _dbContext.Articles.Add(article_SqlServer);
        await _dbContext.SaveChangesAsync();

        return _converter.ToEntity<Article>(article_SqlServer);
    }



    public async Task AddArticlesRangeAsync(params ArticleToCreate[] articles)
    {
        Article_SqlServer[] articles_SqlServer = _converter.ToEntities<Article_SqlServer>(articles).ToArray();

        _dbContext.Articles.AddRange(articles_SqlServer);

        await _dbContext.SaveChangesAsync();
    }



    public async Task<Article> ChangeArticleStateAsync(
        DateTime nowUtc,
        string moderatorId,
        ArticleStatesEnum decision,
        int articleId,
        short versionId
    )
    {
        Article_SqlServer article = await _dbContext.Articles.FindAsync(articleId)
            ?? throw new EntityNotFoundException();
        EntityEntry<Article_SqlServer> articleEntry = _dbContext.Entry<Article_SqlServer>(article);

        articleEntry.Property(ar => ar.ArticleStateId_LastChangedBy_ModeratorId)
            .CurrentValue = moderatorId;

        articleEntry.Property(ar => ar.ArticleStateId)
            .CurrentValue = decision;

        articleEntry.Property(ar => ar.ArticleStateId_LastChangedAt_DateUtc)
            .CurrentValue = nowUtc;

        articleEntry.Property(ar => ar.VersionId)
            .AddProperVersionIdCheck(versionId)
            .IncrementVersionId();

        await _dbContext.SaveChangesAsync();
        return _converter.ToEntity<Article>(article);
    }



    public async Task<Article[]> GetAllArticlesByStateAsync(params ArticleStatesEnum[] states)
    {
        Article_SqlServer[] articles_SqlServer = await _dbContext.Articles
            .Where(ar => states.Contains(ar.ArticleStateId))
            .Where(ar => ar.SoftDeletedAt_DateUtc == null)
            .ToArrayAsync();

        IEnumerable<Article> articles = _converter.ToEntities<Article>(articles_SqlServer);
        return articles.ToArray();
    }



    public async Task<Article[]> GetAllArticlesDecidedByModeratorAsync(string moderatorId)
    {
        Article_SqlServer[] articles_SqlServer = await _dbContext.Articles
            .Where(ar => ar.ArticleStateId_LastChangedBy_ModeratorId == moderatorId)
            .ToArrayAsync();

        IEnumerable<Article> articles = _converter.ToEntities<Article>(articles_SqlServer);
        return articles.ToArray();
    }



    public async Task<Article[]> GetAllUsersArticlesAsync(string userId)
    {
        Article_SqlServer[] articles_SqlServer = await _dbContext.Articles
            .Where(ar => ar.AuthorId == userId)
            .Where(ar => ar.SoftDeletedAt_DateUtc == null)
            .ToArrayAsync();

        IEnumerable<Article> articles = _converter.ToEntities<Article>(articles_SqlServer);
        return articles.ToArray();
    }



    public async Task<Article[]> GetArticlesByIdAndStateAsync(
        ArticleStatesEnum[] states,
        params int[] articleIds
    )
    {
        Article_SqlServer[] articles_SqlServer = await _dbContext.Articles
            .Where(ar => articleIds.Contains(ar.Id))
            .Where(ar => states.Contains(ar.ArticleStateId))
            .Where(ar => ar.SoftDeletedAt_DateUtc == null)
            .ToArrayAsync();

        IEnumerable<Article> articles = _converter.ToEntities<Article>(articles_SqlServer);
        return articles.ToArray();
    }



    public async Task<Article[]> GetArticlesByIdAsync(params int[] articleIds)
    {
        ArticleStatesEnum[] allArticleStates = Enum.GetValues<ArticleStatesEnum>();

        return await GetArticlesByIdAndStateAsync(
            allArticleStates,
            articleIds
        );
    }



    public async Task<Article[]> GetMyArticlesByIdAsync(
        string userId,
        params int[] articleIds
    )
    {
        Article_SqlServer[] articles_SqlServer = await _dbContext.Articles
            .Where(ar => ar.AuthorId == userId)
            .Where(ar => articleIds.Contains(ar.Id))
            .Where(ar => ar.SoftDeletedAt_DateUtc == null)
            .ToArrayAsync();

        IEnumerable<Article> articles = _converter.ToEntities<Article>(articles_SqlServer);
        return articles.ToArray();
    }



    public async Task<int> HardDeleteArticlesAsync(DateTime targetOlderThanDate_Utc)
    {
        Article_SqlServer[] articles = await _dbContext.Articles
            .Where(ar => ar.SoftDeletedAt_DateUtc < targetOlderThanDate_Utc)
            .Where(ar => ar.SoftDeletionReason_ReasonId == SoftDeletionReasonEnum.ArticleSoftDeleted)
            .ToArrayAsync();

        _dbContext.RemoveRange(articles);

        await _dbContext.SaveChangesAsync();

        return articles.Length;
    }



    public async Task ReinstateArticleAsync(
        int articleId,
        short versionId
    )
    {
        Article_SqlServer article = await _dbContext.FindAsync<Article_SqlServer>(articleId)
            ?? throw new EntityNotFoundException();
        EntityEntry<Article_SqlServer> articleEntry = _dbContext.Entry(article);

        articleEntry.Property(ar => ar.SoftDeletedAt_DateUtc)
            .CurrentValue = null;

        articleEntry.Property(ar => ar.SoftDeletionReason_ReasonId)
            .CurrentValue = null;

        articleEntry.Property(ar => ar.VersionId)
            .AddProperVersionIdCheck(versionId)
            .IncrementVersionId();

        await _dbContext.SaveChangesAsync();
    }



    public async Task<Article> SoftDeleteArticleAsync(
        int articleId,
        DateTime nowUtc,
        short versionId
    )
    {
        Article_SqlServer article = await _dbContext.FindAsync<Article_SqlServer>(articleId)
            ?? throw new EntityNotFoundException();
        EntityEntry<Article_SqlServer> articleEntry = _dbContext.Entry(article);

        articleEntry.Property(ar => ar.SoftDeletedAt_DateUtc)
            .CurrentValue = nowUtc;

        articleEntry.Property(ar => ar.SoftDeletionReason_ReasonId)
            .CurrentValue = SoftDeletionReasonEnum.ArticleSoftDeleted;

        articleEntry.Property(ar => ar.VersionId)
            .AddProperVersionIdCheck(versionId)
            .IncrementVersionId();

        await _dbContext.SaveChangesAsync();

        return _converter.ToEntity<Article>(article);
    }



    public async Task<IEnumerable<PersonalData_Article>> UserDownloadsPersonalDataAsync(string userId)
    {
        List<Article_SqlServer> articles_SqlServer = await _dbContext.Articles
            .Where(ar => ar.AuthorId == userId)
            .ToListAsync();

        List<PersonalData_Article> articles = new List<PersonalData_Article>(articles_SqlServer.Count);
        articles_SqlServer.ForEach(ar =>
        {
            articles.Add(
                new PersonalData_Article(
                    id: ar.Id,
                    title: ar.Title,
                    text: ar.Text,
                    submittedAt_Date: ar.SubmittedAt_DateUtc,
                    articleState: Enum.GetName(ar.ArticleStateId)!,
                    articleState_LastChangedAt_Date: ar.ArticleStateId_LastChangedAt_DateUtc
                )
            );
        });

        return articles;
    }
}