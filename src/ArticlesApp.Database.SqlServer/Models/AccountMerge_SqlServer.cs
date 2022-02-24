using System.ComponentModel.DataAnnotations;



namespace ArticlesApp.Database.SqlServer.Models;



public class AccountMerge_SqlServer
{
    public AccountMerge_SqlServer(
        string primaryUserId,
        string secondaryUserId
    )
    {
        PrimaryUserId = primaryUserId;
        SecondaryUserId = secondaryUserId;

        CreatedAt_DateUtc = DateTime.UtcNow;
    }



    public int Id { get; set; }

    public string PrimaryUserId { get; set; }
    public string SecondaryUserId { get; set; }

    public bool PrimaryUserConfirmed { get; set; }
    public bool SecondaryUserConfirmed { get; set; }

    public DateTime CreatedAt_DateUtc { get; set; }

    [Timestamp]
    public byte[]? Timestamp { get; set; }
}