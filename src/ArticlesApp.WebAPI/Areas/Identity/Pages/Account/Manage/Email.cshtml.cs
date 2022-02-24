using ArticlesApp.Database.Models;
using ArticlesApp.WebAPI._AppConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public partial class EmailModel : PageModel
{
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly AppConfiguration _appConfig;
    private readonly ILogger<EmailModel> _logger;

    public EmailModel(
        UserManager<ApplicationUser_DB> userManager,
        AppConfiguration appConfig,
        ILogger<EmailModel> logger
    )
    {
        _userManager = userManager;
        _appConfig = appConfig;
        _logger = logger;
    }



    public string? Username { get; set; }

    public string? Email { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty]
    public InputModel? Input { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string? NewEmail { get; set; }
    }



    private async Task LoadAsync(ApplicationUser_DB user)
    {
        string? email = await _userManager.GetEmailAsync(user);

        Email = email;
        Input = new InputModel
        {
            NewEmail = email
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
    }



    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("Unable to find the user {UserId}.", _userManager.GetUserId(User));

            return NotFound($"Unable to find the user.");
        }

        await LoadAsync(user);
        return Page();
    }



    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        if (_appConfig.UserAccounts.EnableLocalAccounts == false)
        {
            return RedirectToPage("/Account/Manage/Index");
        }

        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("Unable to find the user {UserId}.", _userManager.GetUserId(User));

            return NotFound($"Unable to find the user.");
        }

        if (ModelState.IsValid == false)
        {
            await LoadAsync(user);

            return Page();
        }

        if (Input!.NewEmail == user.Email)
        {
            StatusMessage = "The new email address you submitted was the same as the old one.";
            return RedirectToPage();
        }

        IdentityResult emailResult = await _userManager.SetEmailAsync(
            user,
            Input.NewEmail
        );

        if (emailResult.Succeeded)
        {
            StatusMessage = "You successfully changed the email address.";
            return RedirectToPage();
        }

        if (
            emailResult.Errors != null
            && emailResult.Errors.Any()
        )
        {
            StatusMessage = emailResult.Errors.First().Description;
            return RedirectToPage();
        }
        else
        {
            return RedirectToPage();
            // Shouldn't fall through to here.
        }
    }
}