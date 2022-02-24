namespace ArticlesApp.Core.Entities.Notification;



public class NotificationType
{
    public NotificationTypesEnum Id { get; set; } // This setter exists solely for EF Core.

    public string Name
    {
        get => Enum.GetName<NotificationTypesEnum>(this.Id)!;
        set => _ = value; // no op. This setter exists solely for EF Core and it's LookupTables fill.
    }
}