namespace ArticlesApp.Core.Entities.SoftDeletion;



public class SoftDeletionReason
{
    public SoftDeletionReasonEnum Id { get; set; } // This setter exists solely for EF Core.

    public string Name
    {
        get => Enum.GetName<SoftDeletionReasonEnum>(this.Id)!;
        set => _ = value; // no op. This setter exists solely for EF Core and it's LookupTables fill.
    }
}