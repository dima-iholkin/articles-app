using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account;



[Authorize]
public class ReinstateModel : PageModel
{
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ReinstateModel> _logger;

    public ReinstateModel(
        UserManager<ApplicationUser_DB> userManager,
        IUserRepository userRepository,
        ILogger<ReinstateModel> logger
    )
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _logger = logger;
    }

    public string? ReturnUrl { get; set; }



    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/Identity/Account/Manage/Index";
    }



    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("Unable to find the user {UserId}.", _userManager.GetUserId(User));

            return NotFound($"Unable to find the user.");
        }

        await _userRepository.ReinstateUserByIdAsync(user.Id);
        _logger.LogInformation("User account {UserId} was recovered.", user.Id);

        returnUrl ??= "/Identity/Account/Manage/Index";
        // If no returnUrl provided, return to the Account Manage page to show that the account was recovered.

        return LocalRedirect(returnUrl);
    }
}