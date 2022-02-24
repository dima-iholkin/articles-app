using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.PersonalData;
using ArticlesApp.Core.Orchestrators.Models.Articles;



namespace ArticlesApp.Core.Orchestrators.Infrastructure;



public interface IArticlesRepository
{
    public Task<Article> AddArticleAsync(ArticleToCreate article);

    public Task AddArticlesRangeAsync(params ArticleToCreate[] articles);

    public Task<Article> ChangeArticleStateAsync(
        DateTime nowUtc,
        string moderatorId,
        ArticleStatesEnum decision,
        int articleId,
        short versionId
    );

    public Task<Article[]> GetAllArticlesByStateAsync(params ArticleStatesEnum[] states);

    public Task<Article[]> GetAllArticlesDecidedByModeratorAsync(string moderatorId);

    public Task<Article[]> GetAllUsersArticlesAsync(string userId);

    public Task<Article[]> GetArticlesByIdAndStateAsync(
        ArticleStatesEnum[] states,
        params int[] articleIds
    );

    public Task<Article[]> GetArticlesByIdAsync(params int[] articleIds);

    public Task<Article[]> GetMyArticlesByIdAsync(
        string userId,
        params int[] articleIds
    );

    public Task<int> HardDeleteArticlesAsync(DateTime targetOlderThan_Utc);

    public Task ReinstateArticleAsync(
        int articleId,
        short versionId
    );

    public Task<Article> SoftDeleteArticleAsync(
        int articleId,
        DateTime nowUtc,
        short versionId
    );

    public Task<IEnumerable<PersonalData_Article>> UserDownloadsPersonalDataAsync(string userId);
}