using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Database.Models;
using ArticlesApp.WebAPI._AppConfiguration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account;



[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly AppConfiguration _appConfig;
    private readonly ILogger<LoginModel> _logger;
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly IUserRepository _userRepository;

    public LoginModel(
        AppConfiguration appConfig,
        ILogger<LoginModel> logger,
        SignInManager<ApplicationUser_DB> signInManager,
        UserManager<ApplicationUser_DB> userManager,
        IUserRepository userRepository
    )
    {
        _appConfig = appConfig;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }



    [TempData]
    public string? ErrorMessage { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    [BindProperty]
    public InputModel? Input { get; set; }

    public string? ReturnUrl { get; set; }



    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }



    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        // Edge case #1
        // If the user claims are present in the request.
        // Redirect to the Manage Account page.
        if (User.Identity!.IsAuthenticated)
        {
            ApplicationUser_DB? user = await _userManager.GetUserAsync(User);

            // Edge case #2
            // If the user claims are present, but the user cannot be identified or found.
            // Clear the present identity claims and redisplay the page.
            if (user == null)
            {
                _logger.LogWarning(
                    "Unable to find the user {UserId} for the provided claims.",
                    _userManager.GetUserId(User)
                );
                // Clear the present identity claims to ensure a clean login process:
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Page();
            }

            _logger.LogWarning(
                "User {UserId} requested the Login page, while they were logged-in already.",
                user.Id
            );
            return RedirectToPage("/Account/Manage/Index");
        }

        if (ErrorMessage.IsPresent())
        {
            ModelState.AddModelError(
                string.Empty,
                ErrorMessage!
            );
        }
        IEnumerable<AuthenticationScheme> authenticationSchemes =
            await _signInManager.GetExternalAuthenticationSchemesAsync();
        ExternalLogins = authenticationSchemes.ToList();
        // Clear the existing external cookie to ensure a clean login process:
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        return Page();
    }



    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        // Edge case #1
        // If the user claims are present in the request.
        // Redirect to the Manage Account page.
        if (User.Identity!.IsAuthenticated)
        {
            ApplicationUser_DB? _user = await _userManager.GetUserAsync(User);

            // Edge case #2
            // If the user claims are present, but the user cannot be identified or found.
            // Clear the present identity claims and redisplay the page.
            if (_user == null)
            {
                _logger.LogWarning(
                    "Unable to find the user {UserId} for the provided claims.",
                    _userManager.GetUserId(User)
                );
                // Clear the present identity claims to ensure a clean login process:
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Page();
            }

            _logger.LogWarning(
                "User {UserId} POSTed to Login page, while they were logged-in already.",
                _userManager.GetUserId(User)
            );
            return RedirectToPage("/Account/Manage/Index");
        }

        // Edge case #3
        // If a local login attempted, while the local accounts are disabled on the server.
        // Redisplay the page.
        if (_appConfig.UserAccounts.EnableLocalAccounts == false)
        {
            _logger.LogWarning(
                "User {UserId} POSTed to Login page, while the local accounts are disabled.",
                _userManager.GetUserId(User)
            );
            return Page();
        }

        IEnumerable<AuthenticationScheme> authenticationSchemes =
            await _signInManager.GetExternalAuthenticationSchemesAsync();
        ExternalLogins = authenticationSchemes.ToList();

        // Edge case #4
        // If the submitted Input was invalid.
        // Redisplay the page.
        if (ModelState.IsValid == false)
        {
            _logger.LogWarning(
                "Invalid login form for Email {Email} was submitted.",
                Input!.Email
            );
            return Page();
        }

        ApplicationUser_DB? user = await _userManager.FindByEmailAsync(Input!.Email);

        // Edge case #5
        // If no user found for the provided Email.
        // Redisplay the page with an error message.
        if (user == null)
        {
            _logger.LogWarning(
                "Email {Email} submitted for login was not found.",
                Input.Email
            );
            ModelState.AddModelError(
                string.Empty,
                "Invalid login attempt."
            );
            return Page();
        }

        SignInResult result = await _signInManager.PasswordSignInAsync(
            user,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: true
        );

        // Edge case #6
        // If the user account is locked out.
        // Redirect to the Lockout page.
        if (result.IsLockedOut)
        {
            _logger.LogWarning(
                "{UserId} login attempt with local accounts, while the account is locked out.",
                user.Id
            );
            return RedirectToPage("./Lockout");
        }

        // Edge case #7
        // If the login attempt didnot succeed, likely due to a wrong Email and Password combo.
        // Redisplay the page with an error message.
        if (result.Succeeded == false)
        {
            _logger.LogWarning(
                "User {UserId} invalid login attempt.",
                user.Id
            );
            ModelState.AddModelError(
                string.Empty,
                "Invalid login attempt."
            );
            return Page();
        }

        _logger.LogInformation(
            "User {UserId} was logged-in.",
            user.Id
        );

        // Edge case #8
        // If the user is soft-deleted.
        // Continue with the logged-in credentials, redirect to the Account Recovery page.
        bool isSoftDeleted = await _userRepository.IsSoftDeletedAsync(user.Id);
        if (isSoftDeleted)
        {
            _logger.LogWarning(
                "User {UserId} login attempt, white the account is soft-deleted.",
                user.Id
            );
            return RedirectToPage(
                Routes.AccountRecovery_Relative,
                new { ReturnUrl = returnUrl }
            );
        }

        // After a successful login, redirect to the provided returnUrl:
        return LocalRedirect(returnUrl);
    }
}