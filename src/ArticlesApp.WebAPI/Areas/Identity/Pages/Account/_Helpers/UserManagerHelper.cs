using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Identity;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Helpers;



public class UserManagerHelper
{
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly ILogger<UserManagerHelper> _logger;

    public UserManagerHelper(
        UserManager<ApplicationUser_DB> userManager,
        ILogger<UserManagerHelper> logger
    )
    {
        _userManager = userManager;
        _logger = logger;
    }



    public async Task AddUserToRoleAsync(
        ApplicationUser_DB user,
        string roleName
    )
    {
        await _userManager.AddToRoleAsync(
            user,
            roleName
        );

        _logger.LogInformation("User {UserId} was added to the role {RoleName}.", user.Id, roleName);
    }
}
