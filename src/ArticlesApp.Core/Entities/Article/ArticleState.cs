namespace ArticlesApp.Core.Entities.Article;



public class ArticleState
{
    public ArticleStatesEnum Id { get; set; } // This setter exists solely for EF Core.

    public string Name
    {
        get => Enum.GetName<ArticleStatesEnum>(this.Id)!;
        set => _ = value; // no op. This setter exists solely for EF Core and it's LookupTables fill.
    }
}