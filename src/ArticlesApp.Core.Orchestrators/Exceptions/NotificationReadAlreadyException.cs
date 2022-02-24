namespace ArticlesApp.Core.Orchestrators.Exceptions;



[Serializable]
public class NotificationReadAlreadyException : Exception
{
    public NotificationReadAlreadyException() : base("Notification was read already.")
    { }

    public NotificationReadAlreadyException(string message) : base(message)
    { }

    public NotificationReadAlreadyException(
        string message,
        Exception inner
    ) : base(message, inner)
    { }

    protected NotificationReadAlreadyException(
        SerializationInfo info,
        StreamingContext context
    ) : base(info, context)
    { }
}