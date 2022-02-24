using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public class PersonalDataModel : PageModel
{
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly ILogger<PersonalDataModel> _logger;

    public PersonalDataModel(
        UserManager<ApplicationUser_DB> userManager,
        ILogger<PersonalDataModel> logger
    )
    {
        _userManager = userManager;
        _logger = logger;
    }



    public async Task<IActionResult> OnGet()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("Unable to find the user {UserId}.", _userManager.GetUserId(User));

            return NotFound($"Unable to find the user.");
        }

        return Page();
    }
}