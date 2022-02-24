using ArticlesApp.Core.Entities.Identity;
using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Database.Models;
using ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account;



[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly ILogger<ExternalLoginModel> _logger;
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly UserManagerHelper _userManagerHelper;
    private readonly UsersOrchestrator _usersOrchestrator;

    public ExternalLoginModel(
        ILogger<ExternalLoginModel> logger,
        SignInManager<ApplicationUser_DB> signInManager,
        UserManager<ApplicationUser_DB> userManager,
        UserManagerHelper userManagerHelper,
        UsersOrchestrator usersOrchestrator
    )
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _userManagerHelper = userManagerHelper;
        _usersOrchestrator = usersOrchestrator;
    }



    [TempData]
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public InputModel? Input { get; set; }

    public string? ProviderDisplayName { get; set; }

    public string? ReturnUrl { get; set; }

    [Display(Name = "Default username")]
    public string? Username { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }



    public class InputModel
    {
        [Required]
        [RegularExpression(
            "^[a-zA-Z0-9\\.@\\-_ ]*$",
            ErrorMessage = "Username allowed characters are a-z, A-Z, 0-9, " +
            ". , @ , - , _ , &#160&#160&#160(space)."
        )]
        [StringLength(
            256,
            MinimumLength = 4,
            ErrorMessage = "Username length should be between 4 and 256 characters."
        )]
        [Display(Name = "New username")]
        public string? UserName { get; set; }
    }



    public IActionResult OnGet()
    {
        return RedirectToPage("./Login");
    }



    public async Task<IActionResult> OnGetCallbackAsync(
        string? returnUrl = null,
        string? remoteError = null
    )
    {
        returnUrl ??= Url.Content("~/");

        // Edge case #1
        // If there was a remove error at the 3rd party provider.
        // Redirect to the Login page, and show the remote error message.
        if (remoteError != null)
        {
            _logger.LogWarning(
                "Error from the external login provider {RemoteError}.",
                remoteError
            );
            ErrorMessage = $"Error from the external login provider: {remoteError}";
            return RedirectToPage(
                "./Login",
                new { ReturnUrl = returnUrl }
            );
        }

        ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();
        // Edge case #2
        // If there was an error loading the external login provider information.
        // Redirect to the Login page, and show an error message.
        if (info == null)
        {
            _logger.LogWarning("Error loading external login provider information.");
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage(
                "./Login",
                new { ReturnUrl = returnUrl }
            );
        }

        SignInResult externalLoginResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        // Edge case #3
        // If the user is locked out currently.
        // Redirect to the LockOut page.
        if (externalLoginResult.IsLockedOut)
        {
            ApplicationUser_DB? _user = await _userManager.FindByLoginAsync(
                info.LoginProvider,
                info.ProviderKey
            );
            // If the local user account was not found. It's very unlikely.
            // Redirect to the Login page, and show an error message.
            if (_user == null)
            {
                _logger.LogError(
                    "Local user account {UserId} was not found.",
                    _userManager.GetUserId(User)
                );
                ErrorMessage = "Local user account was not found.";
                return RedirectToPage(
                    "./Login",
                    new { ReturnUrl = returnUrl }
                );
            }

            _logger.LogWarning(
                "User {UserId} attempted login with the {LoginProvider} provider, " +
                    "while the account was locked out.",
                _user.Id,
                info.LoginProvider
            );
            return RedirectToPage("./Lockout");
        }

        // Edge case #4
        // If the current user is not allowed to log-in.
        // Redirect to the Login page, tell about the error.
        if (externalLoginResult.IsNotAllowed)
        {
            ApplicationUser_DB? _user = await _userManager.FindByLoginAsync(
                info.LoginProvider,
                info.ProviderKey
            );
            // If the local user account was not found. It's very unlikely.
            // Redirect to the Login page, and show an error message.
            if (_user == null)
            {
                _logger.LogError(
                    "Local user account {UserId} was not found.",
                    _userManager.GetUserId(User)
                );
                ErrorMessage = "Local user account was not found.";
                return RedirectToPage(
                    "./Login",
                    new { ReturnUrl = returnUrl }
                );
            }

            _logger.LogWarning(
                "User {UserId} not allowed to log-in.",
                _user.Id
            );
            ErrorMessage = "User not allowed to log-in.";
            return RedirectToPage(
                "./Login",
                new { ReturnUrl = returnUrl }
            );
        }

        // Edge case #5
        // If the login did not succeed, probably because the local account does not exist.
        // Register the new local account with the external login info.
        if (externalLoginResult.Succeeded == false)
        {
            string? name = info.Principal.FindFirstValue(ClaimTypes.Name);
            // Edge case #6
            // If the external login provider did not provide the name claim.
            // Generate a new random username.
            if (name == null)
            {
                name = "new_user_" + Guid.NewGuid();
                _logger.LogError(
                    "{LoginProvider} didn't provide the name claim. Generated a new username " +
                        "{Username}.",
                    info.LoginProvider,
                    name
                );
            }

            ApplicationUser_DB _user = new ApplicationUser_DB { UserName = name };
            IdentityResult registrationResult = await _usersOrchestrator.CreateUserAsync(_user);
            // Edge case #7
            // If the registeration failed.
            // Tell the user about an error, and redirect to the Register page.
            if (registrationResult.Succeeded == false)
            {
                _logger.LogWarning(
                    "{Username} registration failed from external login provider {LoginProvider}.",
                    name,
                    info.LoginProvider
                );
                ErrorMessage = "Error during registering the user.";
                return RedirectToPage(
                    "./Register",
                    new { ReturnUrl = returnUrl }
                );
            }

            string roleName_User = nameof(IdentityRolesEnum.User);
            try
            {
                await _userManagerHelper.AddUserToRoleAsync(
                    _user,
                    roleName_User
                );
            }
            catch (Exception)
            {
                _logger.LogError(
                    "User {UserId} failed to be added to role {RoleName}.",
                    _user.Id,
                    roleName_User
                );
            }

            IdentityResult addExternalLoginResult = await _userManager.AddLoginAsync(
                _user,
                info
            );
            // Edge case #8
            // If adding the external login to the registered user failed.
            // Remove the created user, tell the user about an error, and redirect to the Register
            // page.
            if (addExternalLoginResult.Succeeded == false)
            {
                _logger.LogError(
                    "Error during adding the {ExternalProvider} external login info to the new " +
                        "registered user {UserId}.",
                    info.LoginProvider,
                    _user.Id
                );
                await _userManager.DeleteAsync(_user);
                ErrorMessage = "Error during the user registration. Please try again later.";
                return RedirectToPage(
                    "./Register",
                    new { ReturnUrl = returnUrl }
                );
            }

            _logger.LogInformation(
                "User {UserId} registered with an external login provider {ExternalProvider}.",
                _user.Id,
                info.LoginProvider
            );
            // Log-in the registered user:
            await _signInManager.SignInAsync(
                _user,
                isPersistent: false,
                info.LoginProvider
            );
            StatusMessage = $"User was successfully registered with an external login from " +
                $"{info.LoginProvider}.";
            return RedirectToPage(
                "/Account/Manage/Index",
                new { ReturnUrl = returnUrl }
            );
        }

        ApplicationUser_DB? user = await _userManager.FindByLoginAsync(
            info.LoginProvider,
            info.ProviderKey
        );
        // If the local user account was not found. It's very unlikely.
        // Redirect to the Login page, and show an error message.
        if (user == null)
        {
            _logger.LogError(
                "Local user account {UserId} was not found.",
                _userManager.GetUserId(User)
            );
            ErrorMessage = "Local user account was not found.";
            return RedirectToPage(
                "./Login",
                new { ReturnUrl = returnUrl }
            );
        }

        _logger.LogInformation(
            "User {UserId} logged-in with the {LoginProvider} provider.",
            user.Id,
            info.LoginProvider
        );
        return LocalRedirect(returnUrl);
    }



    public IActionResult OnPost(
        string provider,
        string? returnUrl = null
    )
    {
        // Request a redirect to the external login provider, with a callback to the
        // OnGetCallbackAsync method, with the previous ReturnUrl embedded.
        string? redirectUrl = Url.Page(
            "./ExternalLogin",
            pageHandler: "Callback",
            values: new { returnUrl }
        );
        AuthenticationProperties properties =
            _signInManager.ConfigureExternalAuthenticationProperties(
                provider,
                redirectUrl
            );
        return new ChallengeResult(
            provider,
            properties
        );
    }
}