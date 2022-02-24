using Microsoft.AspNetCore.Identity;



namespace ArticlesApp.Database.Models;



public class ApplicationUserRole_DB : IdentityRole
{
    public ApplicationUserRole_DB() : base()
    { }

    public ApplicationUserRole_DB(string name) : base(name)
    { }
}