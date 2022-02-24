using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Core.Orchestrators.Results.Articles;



namespace ArticlesApp.Core.Orchestrators.Validators;



public static class ArticleValidator
{
    public static ArticleValidationResult Validate(this ArticleToCreate_Submitted article)
    {
        if (
            String.IsNullOrEmpty(article.Title)
            || String.IsNullOrEmpty(article.Text)
        )
        {
            return new ArticleValidationResult()
            {
                Success = false
            };
        }

        return new ArticleValidationResult()
        {
            Success = true,
            ArticleValidated = new ArticleToCreate_Validated(
                article.Title,
                article.Text
            )
        };
    }
}