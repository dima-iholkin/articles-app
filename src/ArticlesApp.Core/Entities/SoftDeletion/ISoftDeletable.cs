namespace ArticlesApp.Core.Entities.SoftDeletion;



public interface ISoftDeletable
{
    public DateTime? SoftDeletedAt_DateUtc { get; set; }
}