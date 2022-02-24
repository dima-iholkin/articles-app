using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer.Models;
using ArticlesApp.WebAPI.IdentityService.Services.AccountMerge;
using ArticlesApp.WebAPI.IdentityService.Services.AccountMerge.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OneOf;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public class ExternalLoginsModel : PageModel
{
    private readonly AccountMergeManager _accountMergeManager;
    private readonly ILogger<ExternalLoginsModel> _logger;
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;

    public ExternalLoginsModel(
        AccountMergeManager accountMergeManager,
        ILogger<ExternalLoginsModel> logger,
        SignInManager<ApplicationUser_DB> signInManager,
        UserManager<ApplicationUser_DB> userManager
    )
    {
        _accountMergeManager = accountMergeManager;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }



    public IList<UserLoginInfo>? CurrentLogins { get; set; }

    public IList<AuthenticationScheme>? OtherLogins { get; set; }

    public bool ShowRemoveButton { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }



    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        // Edge case #1
        // If the user cannot be identified or found.
        // Redirect to the Login page.
        if (user == null)
        {
            _logger.LogWarning(
                "Unable to find the user {UserId}.",
                _userManager.GetUserId(User)
            );
            return RedirectToPage("/Account/Login");
        }

        CurrentLogins = await _userManager.GetLoginsAsync(user);

        IEnumerable<AuthenticationScheme> authenticationSchemes =
            await _signInManager.GetExternalAuthenticationSchemesAsync();
        OtherLogins = authenticationSchemes
            .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();

        bool isLocalUser = user.PasswordHash != null;
        bool userHasMoreThanOneExternalLogin = CurrentLogins.Count > 1;
        ShowRemoveButton = (
            isLocalUser
            || userHasMoreThanOneExternalLogin
        );

        return Page();
    }



    public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
    {
        ApplicationUser_DB? currentUser = await _userManager.GetUserAsync(User);
        // Edge case #1
        // If the user cannot be identified or found.
        // Redirect to the Login page.
        if (currentUser == null)
        {
            _logger.LogWarning(
                "Unable to find the user {UserId}.",
                _userManager.GetUserId(User)
            );
            return RedirectToPage("/Account/Login");
        }

        ExternalLoginInfo? externalLoginInfo =
            await _signInManager.GetExternalLoginInfoAsync(currentUser.Id);
        // Edge case #2
        // If the ExternalLoginInfo for the user wasn't found, who knows why.
        // Show the error message and redisplay the page.
        if (externalLoginInfo == null)
        {
            _logger.LogWarning(
                "Unexpected error occurred loading the external login info for User {UserId}.",
                currentUser.Id
            );
            StatusMessage = "Unexpected error occurred loading the external login info.";
            return RedirectToPage();
        }

        ApplicationUser_DB? externalUser = await _userManager.FindByLoginAsync(
            externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey
        );
        // Edge case #3
        // If the external login belongs to a different registered user, not the current
        // logged-in user.
        // Create an AccountMerge record in the database, and redirect to the Account Merge
        // page.
        if (
            externalUser != null
            && externalUser.Id != currentUser.Id
        )
        {
            OneOf<AccountMerge_SqlServer, RecordWithOppositeMergeDirectionPresent,
                WorkflowRulesViolation>
                accountMergeResult = await _accountMergeManager.CreateAsync(
                    currentUser.Id,
                    externalUser.Id
                );

            // If a record with the opposite merge direction present.
            // Tell the user about it, and redirect to this record's AccountMerge page.
            if (accountMergeResult.IsT1)
            {
                RecordWithOppositeMergeDirectionPresent recordWithOppositeMergeDirection =
                    accountMergeResult.AsT1;
                StatusMessage = "AccountMerge request from the second user account " +
                    "to this user account is present already. " +
                    "You have to Confirm or Reject that merge request first.";
                return RedirectToPage(
                    "/Account/Manage/AccountMerge",
                    new { Id = recordWithOppositeMergeDirection.AccountMergeId }
                );
            }

            // If the workflow rules were violated.
            // Tell the user about an error, and redirect to the current page.
            if (accountMergeResult.IsT2)
            {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                WorkflowRulesViolation workflowRulesViolation = accountMergeResult.AsT2;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                StatusMessage = "Something went wrong. Please try again later.";
                return RedirectToPage();
            }

            // If the CancelResult type is not one of the expected ones.
            // Tell the user about an error and redirect to the AccountMergeList page.
            if (accountMergeResult.IsT0 == false)
            {
                _logger.LogError(
                    "Unexpected CancelResult type {CancelResultType} returned.",
                    accountMergeResult.Value.GetType()
                );
                StatusMessage = "Something went wrong. Please try again later.";
                return RedirectToPage();
            }

            AccountMerge_SqlServer accountMergeRecord = accountMergeResult.AsT0;
            _logger.LogInformation(
                "User {UserId} created an AccountMerge record {AccountMergeId}.",
                currentUser.Id,
                accountMergeRecord.Id
            );
            StatusMessage = "This external login belongs to a different registered " +
                "user. You can merge both user accounts into the current one. " +
                "All the data will be preserved.";
            return RedirectToPage(
                "/Account/Manage/AccountMerge",
                new { Id = accountMergeRecord.Id }
            );
        }

        IdentityResult result = await _userManager.AddLoginAsync(
            currentUser,
            externalLoginInfo
        );
        // Edge case #4
        // If the external login did not succeed, for whatever reason.
        // Show the error message and redisplay the page.
        if (result.Succeeded == false)
        {
            _logger.LogError(
                "Something went wrong when adding an external login for User {UserId}.",
                currentUser.Id
            );
            StatusMessage = "Something went wrong when adding an external login. " +
                "Please try again later.";
            return RedirectToPage();
        }

        // Clear the present external cookie to ensure a clean login process:
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        StatusMessage = "The external login was added.";
        return RedirectToPage();
    }



    public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    {
        // Clear the present external cookie to ensure a clean login process:
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user:
        string? redirectUrl = Url.Page(
            "./ExternalLogins",
            pageHandler: "LinkLoginCallback"
        );

        AuthenticationProperties properties =
            _signInManager.ConfigureExternalAuthenticationProperties(
                provider,
                redirectUrl,
                _userManager.GetUserId(User)
            );
        return new ChallengeResult(
            provider,
            properties
        );
    }



    public async Task<IActionResult> OnPostRemoveLoginAsync(
        string loginProvider,
        string providerKey
    )
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        // Edge case #1
        // If the user cannot be identified or found.
        // Redirect to the Login page.
        if (user == null)
        {
            _logger.LogWarning(
                "Unable to find the user {UserId}.",
                _userManager.GetUserId(User)
            );
            return RedirectToPage("/Account/Login");
        }

        IdentityResult result = await _userManager.RemoveLoginAsync(
            user,
            loginProvider,
            providerKey
        );
        // Edge case #2
        // If the external login wasn't removed, for whatever reason.
        // Show an error message and redisplay the page.
        if (result.Succeeded == false)
        {
            _logger.LogWarning(
                "The external login {ProviderKey} from {ProviderName} provided wasn't removed " +
                "for user {UserId}.",
                providerKey,
                loginProvider,
                user.Id
            );
            StatusMessage = "Something went wrong. The external login was not removed.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "The external login was removed.";
        return RedirectToPage();
    }
}