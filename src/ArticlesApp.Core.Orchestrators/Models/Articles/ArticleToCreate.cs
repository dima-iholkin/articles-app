using ArticlesApp.Core.Entities.Article;



namespace ArticlesApp.Core.Orchestrators.Models.Articles;



public class ArticleToCreate
{
    public ArticleToCreate(
        string title,
        string text,
        DateTime submittedAt_DateUtc,
        string authorId,
        ArticleStatesEnum articleStateId
    )
    {
        Title = title;
        Text = text;
        SubmittedAt_DateUtc = submittedAt_DateUtc;
        AuthorId = authorId;
        ArticleStateId = articleStateId;
    }

    public string Title { get; set; }
    public string Text { get; set; }

    public DateTime SubmittedAt_DateUtc { get; set; }

    public string AuthorId { get; set; }

    public ArticleStatesEnum ArticleStateId { get; set; }
}