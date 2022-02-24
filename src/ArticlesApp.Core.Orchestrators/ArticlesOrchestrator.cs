using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Identity;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles;
using ArticlesApp.Core.Orchestrators.Infrastructure.EventBrokers.Articles.Events;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Core.Orchestrators.Results.Articles;
using ArticlesApp.Core.Orchestrators.Validators;
using OneOf;



namespace ArticlesApp.Core.Orchestrators;



public class ArticlesOrchestrator
{
    private readonly IArticlesEventBroker _articlesEventBroker;
    private readonly IArticlesRepository _articlesRepository;

    public ArticlesOrchestrator(
        IArticlesEventBroker articlesEventBroker,
        IArticlesRepository articlesRepository
    )
    {
        _articlesEventBroker = articlesEventBroker;
        _articlesRepository = articlesRepository;
    }



    public async Task<Article[]> GetAllApprovedArticlesAsync()
    {
        return await _articlesRepository.GetAllArticlesByStateAsync(ArticleStatesEnum.Approved);
    }



    public async Task<Article[]> GetAllUsersArticlesAsync(string userId)
    {
        return await _articlesRepository.GetAllUsersArticlesAsync(userId);
    }



    public async Task<Article[]> GetApprovedArticlesByIdAsync(params int[] articleIds)
    {
        ArticleStatesEnum[] articleStates = new ArticleStatesEnum[]
        {
            ArticleStatesEnum.Approved
        };

        Article[] articles = await _articlesRepository.GetArticlesByIdAndStateAsync(
            articleStates,
            articleIds
        );
        return articles;
    }



    public async Task<Article[]> GetUsersArticlesByIdAsync(
        string userId,
        params int[] articleIds
    )
    {
        Article[] articles = await _articlesRepository.GetMyArticlesByIdAsync(
            userId,
            articleIds
        );
        return articles;
    }



    public async Task<Article> ModeratorDecidesOnPendingArticleAsync(
        IdentityRolesEnum[] userRoles,
        string moderatorId,
        ArticleStatesEnum articleDecision,
        int articleId,
        short versionId
    )
    {
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            throw new UnauthorizedUserException();
        }

        Article[] articles = await _articlesRepository.GetArticlesByIdAsync(articleId);
        if (articles.Length == 0)
        {
            throw new EntityNotFoundException();
        }

        Article article = articles[0];
        if (article.ArticleStateId != ArticleStatesEnum.Pending)
        {
            throw new ArticleNotInPendingStateException(
                "The article should be in Pending state before a moderator's decision"
            );
        }

        bool isValidDecision =
            articleDecision == ArticleStatesEnum.Approved
            || articleDecision == ArticleStatesEnum.Rejected;
        if (isValidDecision == false)
        {
            throw new ArgumentException(
                $"The moderator's decision should be one of " +
                    $"{nameof(ArticleStatesEnum.Approved)} or {nameof(ArticleStatesEnum.Rejected)}. " +
                    $"The provided argument was: {articleDecision}.",
                nameof(articleDecision)
            );
        }

        Article articleSaved = await _articlesRepository.ChangeArticleStateAsync(
            DateTime.UtcNow,
            moderatorId,
            articleDecision,
            articleId,
            versionId
        );

        _articlesEventBroker.ArticleModeratorDecisionEvent.Publish(
            this,
            new ArticleArgs(articleSaved)
        );

        return articleSaved;
    }



    public async Task<Article[]> ModeratorGetsAllOwnDecisionsAsync(
        IdentityRolesEnum[] userRoles,
        string moderatorId
    )
    {
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            throw new UnauthorizedUserException();
        }

        Article[] articles = await _articlesRepository.GetAllArticlesDecidedByModeratorAsync(moderatorId);
        return articles;
    }



    public async Task<Article[]> ModeratorGetsAllPendingArticlesAsync(params IdentityRolesEnum[] userRoles)
    {
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            throw new UnauthorizedUserException();
        }

        return await _articlesRepository.GetAllArticlesByStateAsync(ArticleStatesEnum.Pending);
    }



    public async Task<Article[]> ModeratorGetsArticlesByIdsAsync(
        IdentityRolesEnum[] userRoles,
        params int[] articleIds
    )
    {
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            throw new UnauthorizedUserException();
        }

        ArticleStatesEnum[] allArticleStates = Enum.GetValues<ArticleStatesEnum>();

        Article[] articles = await _articlesRepository.GetArticlesByIdAndStateAsync(
            allArticleStates,
            articleIds
        );
        return articles;
    }



    public async Task<Article> UserDeletesOwnArticleAsync(
        int articleId,
        string userId,
        short versionId
    )
    {
        Article[] articles = await _articlesRepository.GetArticlesByIdAsync(articleId);
        if (articles.Length == 0)
        {
            throw new EntityNotFoundException();
        }

        Article article = articles[0];
        if (article.AuthorId != userId)
        {
            throw new UnauthorizedUserException();
        }

        Article articleSaved = await _articlesRepository.SoftDeleteArticleAsync(
            articleId,
            DateTime.UtcNow,
            versionId
        );

        _articlesEventBroker.ArticleSoftDeletedEvent.Publish(
            this,
            new ArticleArgs(articleSaved)
        );

        return articleSaved;
    }



    public async Task<OneOf<Article, ArticleValidationFailed>> UserSubmitsArticleAsync(
        string userId,
        ArticleToCreate_Submitted articleSubmitted
    )
    {
        ArticleValidationResult validationResult = articleSubmitted.Validate();
        if (validationResult.Success == false)
        {
            return new ArticleValidationFailed();
        }

        ArticleToCreate_Validated articleValidated = validationResult.ArticleValidated!;
        ArticleToCreate articleToCreate = new ArticleToCreate(
            title: articleValidated.Title,
            text: articleValidated.Text,
            submittedAt_DateUtc: DateTime.UtcNow,
            authorId: userId,
            articleStateId: ArticleStatesEnum.Pending
        );
        Article articleSaved = await _articlesRepository.AddArticleAsync(articleToCreate);

        _articlesEventBroker.ArticleCreatedEvent.Publish(
            this,
            new ArticleArgs(articleSaved)
        );

        return articleSaved;
    }
}