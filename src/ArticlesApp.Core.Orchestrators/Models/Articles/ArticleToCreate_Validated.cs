namespace ArticlesApp.Core.Orchestrators.Models.Articles;



public class ArticleToCreate_Validated
{
    public ArticleToCreate_Validated(
        string title,
        string text
    )
    {
        Title = title;
        Text = text;
    }

    public string Title { get; set; }
    public string Text { get; set; }
}