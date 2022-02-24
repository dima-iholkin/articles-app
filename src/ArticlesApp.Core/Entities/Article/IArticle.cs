namespace ArticlesApp.Core.Entities.Article;



public interface IArticle
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Text { get; set; }

    public DateTime SubmittedAt_DateUtc { get; set; }

    public string AuthorId { get; set; }

    public ArticleStatesEnum ArticleStateId { get; set; }

    public DateTime? ArticleStateId_LastChangedAt_DateUtc { get; set; }
    public string? ArticleStateId_LastChangedBy_ModeratorId { get; set; }

    public DateTime? SoftDeletedAt_DateUtc { get; set; }

    public short VersionId { get; set; }
}