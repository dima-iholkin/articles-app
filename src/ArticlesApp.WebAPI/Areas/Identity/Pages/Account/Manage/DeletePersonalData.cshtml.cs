using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public class DeletePersonalDataModel : PageModel
{
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly ILogger<DeletePersonalDataModel> _logger;
    private readonly IUserRepository _userRepository;

    public DeletePersonalDataModel(
        UserManager<ApplicationUser_DB> userManager,
        SignInManager<ApplicationUser_DB> signInManager,
        ILogger<DeletePersonalDataModel> logger,
        IUserRepository userRepository
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _userRepository = userRepository;
    }



    [BindProperty]
    public InputModel? Input { get; set; }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public bool RequirePassword { get; set; }



    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        RequirePassword = await _userManager.HasPasswordAsync(user);
        return Page();
    }



    public async Task<IActionResult> OnPostAsync()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        RequirePassword = await _userManager.HasPasswordAsync(user);
        if (RequirePassword)
        {
            bool hasCorrectPassword = await _userManager.CheckPasswordAsync(user, Input?.Password);
            if (hasCorrectPassword == false)
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return Page();
            }
        }

        await _userRepository.SoftDeleteUserByIdAsync(
            user.Id,
            DateTime.UtcNow
        );
        _logger.LogInformation("User '{UserId}' soft-deleted themselves.", user.Id);

        await _signInManager.SignOutAsync();

        return Redirect("~/");
    }
}