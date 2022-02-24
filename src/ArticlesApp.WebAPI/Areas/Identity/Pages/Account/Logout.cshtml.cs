using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account;



[Authorize]
public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(
        SignInManager<ApplicationUser_DB> signInManager,
        ILogger<LogoutModel> logger
    )
    {
        _signInManager = signInManager;
        _logger = logger;
    }



    public void OnGet()
    { }



    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();

        string? userIdClaim = _signInManager.UserManager.GetUserId(User);
        _logger.LogInformation("User {UserId} has logged-out.", userIdClaim);

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToPage();
    }
}