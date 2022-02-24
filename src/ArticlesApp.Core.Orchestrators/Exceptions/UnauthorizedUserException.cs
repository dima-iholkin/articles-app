namespace ArticlesApp.Core.Orchestrators.Exceptions;



[Serializable]
public class UnauthorizedUserException : Exception
{
    public UnauthorizedUserException() : base("User unauthorized to access this resource.")
    { }

    public UnauthorizedUserException(string message) : base(message)
    { }

    public UnauthorizedUserException(
        string message, 
        Exception inner
    ) : base(message, inner)
    { }

    protected UnauthorizedUserException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context)
    { }
}