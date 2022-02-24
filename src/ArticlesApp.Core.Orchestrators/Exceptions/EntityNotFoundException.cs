namespace ArticlesApp.Core.Orchestrators.Exceptions;



[Serializable]
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() : base("Entity was not found.")
    { }

    public EntityNotFoundException(string message) : base(message)
    { }

    public EntityNotFoundException(
        string message,
        Exception inner
    ) : base(message, inner)
    { }

    protected EntityNotFoundException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context)
    { }
}