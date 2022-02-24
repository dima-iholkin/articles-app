namespace ArticlesApp.Core.Orchestrators.Exceptions;



[Serializable]
public class ArticleNotInPendingStateException : Exception
{
    public ArticleNotInPendingStateException() : base("Article is not in the Pending state.")
    { }

    public ArticleNotInPendingStateException(string message) : base(message)
    { }

    public ArticleNotInPendingStateException(
        string message,
        Exception inner
    ) : base(message, inner)
    { }

    protected ArticleNotInPendingStateException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context)
    { }
}