using ArticlesApp.Core.Orchestrators.Models.Articles;



namespace ArticlesApp.Core.Orchestrators.Results.Articles;



public class ArticleValidationResult
{
    public bool Success { get; set; }

    public ArticleToCreate_Validated? ArticleValidated { get; set; }
}