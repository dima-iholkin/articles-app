namespace ArticlesApp.Core.Entities.PersonalData;



public class PersonalData_Article
{
    public PersonalData_Article(
        int id,
        string title,
        string text,
        DateTime submittedAt_Date,
        string articleState,
        DateTime? articleState_LastChangedAt_Date = null
    )
    {
        Id = id;
        Title = title;
        Text = text;
        SubmittedAt_Date = submittedAt_Date;
        ArticleState = articleState;
        ArticleState_LastChangedAt_Date = articleState_LastChangedAt_Date;
    }

    public int Id { get; set; }

    public string Title { get; set; }
    public string Text { get; set; }

    public DateTime SubmittedAt_Date { get; set; }

    public string ArticleState { get; set; }

    public DateTime? ArticleState_LastChangedAt_Date { get; set; }
}