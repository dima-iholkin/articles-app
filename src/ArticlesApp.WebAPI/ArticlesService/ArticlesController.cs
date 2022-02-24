using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Identity;
using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Core.Orchestrators.Results.Articles;
using ArticlesApp.WebAPI.Helpers;
using ArticlesApp.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;



namespace ArticlesApp.WebAPI.ArticlesService;



[Route("api/articles")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly ArticlesOrchestrator _articleOrchestrator;
    private readonly ArticlesConverter_WebAPI _converter;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(
        ArticlesOrchestrator articleOrchestrator,
        ArticlesConverter_WebAPI converter,
        ILogger<ArticlesController> logger
    ) : base()
    {
        _articleOrchestrator = articleOrchestrator;
        _converter = converter;
        _logger = logger;
    }



    [HttpGet]
    public async Task<ActionResult<Article_WebAPI[]>> GetAllApprovedArticles()
    {
        Article[] articles = await _articleOrchestrator.GetAllApprovedArticlesAsync();

        IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
        return Ok(articles_WebAPI.ToArray());
        // GET: api/articles
    }



    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<Article_WebAPI[]>> GetAllMyArticles()
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        Article[] articles = await _articleOrchestrator.GetAllUsersArticlesAsync(userId);

        IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
        return Ok(articles_WebAPI.ToArray());
        // GET: api/articles/my
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Article_WebAPI[]>> GetApprovedArticlesByIds(int id)
    {
        Article[] articles = await _articleOrchestrator.GetApprovedArticlesByIdAsync(id);

        IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
        return Ok(articles_WebAPI.ToArray());
        // GET: api/articles/5
    }



    [HttpGet("my/{id}")]
    [Authorize]
    public async Task<ActionResult<Article_WebAPI[]>> GetMyArticlesByIds(int id)
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        Article[] articles = await _articleOrchestrator.GetUsersArticlesByIdAsync(userId, id);

        IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
        return Ok(articles_WebAPI.ToArray());
        // GET: api/articles/my/5
    }



    [HttpPut("{articleId}")]
    [Authorize]
    public async Task<ActionResult<Article_WebAPI>> ModeratorDecidesOnPendingArticle(
        int articleId,
        [FromQuery(Name = "decision")] string decision,
        [FromQuery(Name = "versionId")] short versionId
    )
    {

        string? moderatorId = User.GetUserId();
        if (moderatorId == null)
        {
            return Unauthorized();
        }

        IdentityRolesEnum[] userRoles = User.GetRoles();
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            return Forbid();
        }

        bool decisionWasParsed = Enum.TryParse<ArticleStatesEnum>
        (
            decision,
            ignoreCase: true,
            out ArticleStatesEnum decisionParsed
        );
        if (decisionWasParsed == false)
        {
            return BadRequest("Decision value was incorrect.");
        }

        try
        {
            Article articleModified = await _articleOrchestrator.ModeratorDecidesOnPendingArticleAsync(
                userRoles,
                moderatorId,
                decisionParsed,
                articleId,
                versionId
            );

            Article_WebAPI article_WebAPI = _converter.ToEntity<Article_WebAPI>(articleModified);
            return Ok(article_WebAPI);
        }
        catch (UnauthorizedUserException)
        {
            return Forbid();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArticleNotInPendingStateException ex)
        {
            return Conflict(ex.Message);
        }
        // PUT: api/articles/5
    }



    [HttpGet("my/decisions")]
    [Authorize]
    public async Task<ActionResult<Article_WebAPI[]>> ModeratorGetsAllOwnDecisions()
    {
        string? moderatorId = User.GetUserId();
        if (moderatorId == null)
        {
            return Unauthorized();
        }

        IdentityRolesEnum[] userRoles = User.GetRoles();
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            return Forbid();
        }

        try
        {
            Article[] articles = await _articleOrchestrator.ModeratorGetsAllOwnDecisionsAsync(
                userRoles,
                moderatorId
            );

            IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
            return Ok(articles_WebAPI.ToArray());
        }
        catch (UnauthorizedUserException)
        {
            return Forbid();
        }
        // GET: api/articles/my/decisions
    }



    [HttpGet("pending")]
    [Authorize]
    public async Task<ActionResult<Article_WebAPI[]>> ModeratorGetsAllPendingArticles()
    {
        string? moderatorId = User.GetUserId();
        if (moderatorId == null)
        {
            return Unauthorized();
        }

        IdentityRolesEnum[] userRoles = User.GetRoles();
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            return Forbid();
        }

        try
        {
            Article[] articles = await _articleOrchestrator.ModeratorGetsAllPendingArticlesAsync(userRoles);

            IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
            return Ok(articles_WebAPI.ToArray());
        }
        catch (UnauthorizedUserException)
        {
            return Forbid();
        }
        // GET: api/articles/pending
    }



    [HttpGet("pending/{id}")]
    [Authorize]
    public async Task<ActionResult<Article_WebAPI[]>> ModeratorGetsArticlesByIds(int id)
    {
        string? moderatorId = User.GetUserId();
        if (moderatorId == null)
        {
            return Unauthorized();
        }

        IdentityRolesEnum[] userRoles = User.GetRoles();
        if (userRoles.Contains(IdentityRolesEnum.Moderator) == false)
        {
            return Forbid();
        }

        try
        {
            Article[] articles = await _articleOrchestrator.ModeratorGetsArticlesByIdsAsync(
                userRoles,
                id
            );

            IEnumerable<Article_WebAPI> articles_WebAPI = _converter.ToEntities<Article_WebAPI>(articles);
            return Ok(articles_WebAPI.ToArray());
        }
        catch (UnauthorizedUserException)
        {
            return Forbid();
        }
        // GET: api/articles/pending/5
    }



    [HttpDelete("{articleId}")]
    [Authorize]
    public async Task<ActionResult> UserDeletesOwnArticle(
        int articleId,
        [FromQuery(Name = "versionId")] short versionId
    )
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            Article articleSaved = await _articleOrchestrator.UserDeletesOwnArticleAsync(
                articleId,
                userId,
                versionId
            );

            Article_WebAPI article_WebAPI = _converter.ToEntity<Article_WebAPI>(articleSaved);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedUserException)
        {
            return Forbid();
        }
        // DELETE: api/articles/5
    }



    [HttpPost]
    [Authorize]
    public async Task<ActionResult> UserSubmitsArticle([FromBody] ArticleToCreate_Submitted article)
    {
        string? userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        OneOf<Article, ArticleValidationFailed> articleSavedResult =
            await _articleOrchestrator.UserSubmitsArticleAsync(
                userId,
                article
            );

        return articleSavedResult.Match<ActionResult>(
            articleSaved => CreatedAtAction(
                nameof(GetMyArticlesByIds),
                new { id = articleSaved.Id },
                _converter.ToEntity<Article_WebAPI>(articleSaved)
            ),
            validationFailed => BadRequest(
                $"Fields {nameof(ArticleToCreate_Submitted.Title)} and {nameof(ArticleToCreate_Submitted.Text)} " +
                $"are required and cannot be empty."
            )
        );
        // POST: api/articles
    }
}