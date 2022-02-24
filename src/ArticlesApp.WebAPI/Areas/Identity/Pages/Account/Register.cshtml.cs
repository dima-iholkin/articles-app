using ArticlesApp.Core.Entities.Identity;
using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Database.Models;
using ArticlesApp.WebAPI._AppConfiguration;
using ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account;



[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly AppConfiguration _appConfig;
    private readonly ILogger<RegisterModel> _logger;
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly UserManagerHelper _userManagerHelper;
    private readonly UsersOrchestrator _usersOrchestrator;

    public RegisterModel(
        SignInManager<ApplicationUser_DB> signInManager,
        UserManagerHelper userManagerHelper,
        UsersOrchestrator usersOrchestrator,
        ILogger<RegisterModel> logger,
        AppConfiguration appConfig
    )
    {
        _signInManager = signInManager;
        _userManagerHelper = userManagerHelper;
        _usersOrchestrator = usersOrchestrator;
        _logger = logger;
        _appConfig = appConfig;
    }



    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    [BindProperty]
    public InputModel? Input { get; set; }

    public string? ReturnUrl { get; set; }



    public class InputModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }



    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToPage("/Account/Manage/Index");
        }

        if (_appConfig.UserAccounts.EnableLocalAccounts == false)
        {
            return RedirectToPage("./Login");
        }

        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToPage("/Account/Manage/Index");
        }

        if (_appConfig.UserAccounts.EnableLocalAccounts == false)
        {
            return RedirectToPage("./Login");
        }

        returnUrl ??= Url.Content("~/");

        if (User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl);
        }

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid == false)
        {
            return Page();
        }

        ApplicationUser_DB user = new ApplicationUser_DB
        {
            UserName = Input!.Email,
            Email = Input.Email
        };
        IdentityResult result = await _usersOrchestrator.CreateUserAsync(
            user,
            Input.Password
        );
        if (result.Succeeded == false)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description
                );
            }

            return Page();
        }
        _logger.LogInformation(
            "User {UserId} registered with a local login.", 
            user.Id
        );

        string roleName_User = Enum.GetName(IdentityRolesEnum.User)!;
        try
        {
            await _userManagerHelper.AddUserToRoleAsync(
                user,
                roleName_User
            );
        }
        catch (Exception)
        {
            _logger.LogError("User {UserId} wasn't added to the {RoleName} role.", user.Id, roleName_User);
        }

        await _signInManager.SignInAsync(
            user,
            isPersistent: false
        );
        return RedirectToPage(
            "/Account/Manage/Index",
            new { ReturnUrl = returnUrl }
        );
    }
}