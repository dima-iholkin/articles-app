using ArticlesApp.Core.Entities.SoftDeletion;
using Microsoft.AspNetCore.Identity;



namespace ArticlesApp.Database.Models;



public class ApplicationUser_DB : IdentityUser, ISoftDeletable
{
    public DateTime? SoftDeletedAt_DateUtc { get; set; }
}