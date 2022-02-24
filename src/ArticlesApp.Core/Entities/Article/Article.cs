using ArticlesApp.Core.Entities.SoftDeletion;



namespace ArticlesApp.Core.Entities.Article;



public class Article : IArticle, ISoftDeletable
{
    public Article(
        int id,
        string title,
        string text,
        DateTime submittedAt_DateUtc,
        string authorId,
        ArticleStatesEnum articleStateId,
        short versionId
    )
    {
        Id = id;
        Title = title;
        Text = text;
        SubmittedAt_DateUtc = submittedAt_DateUtc;
        AuthorId = authorId;
        ArticleStateId = articleStateId;
        VersionId = versionId;
    }

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