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
public class AccountMergeModel : PageModel
{
    private readonly AccountMergeManager _accountMergeManager;
    private readonly ILogger<AccountMergeModel> _logger;
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;

    public AccountMergeModel(
        AccountMergeManager accountMergeManager,
        ILogger<AccountMergeModel> logger,
        SignInManager<ApplicationUser_DB> signInManager,
        UserManager<ApplicationUser_DB> userManager
    )
    {
        _accountMergeManager = accountMergeManager;
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }



    public AccountMerge_SqlServer? AccountMergeRecord { get; set; }

    public string? CurrentUserId { get; set; }

    public ApplicationUser_DB? PrimaryUser { get; set; }

    public IList<UserLoginInfo>? PrimaryUser_CurrentLogins { get; set; }

    public ApplicationUser_DB? SecondaryUser { get; set; }

    public IList<UserLoginInfo>? SecondaryUser_CurrentLogins { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }



    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "Id")] int? accountMergeId)
    {
        CurrentUserId = _userManager.GetUserId(User);

        // Edge case #1
        // If the AccountMergeId not provided.
        // Redirect to the Account Merge List page.
        if (accountMergeId == null)
        {
            _logger.LogWarning(
                "AccountMergeId parameter wasn't provided in method {MethodName}.",
                nameof(OnGetAsync)
            );
            StatusMessage = "AccountMergeId parameter wasn't provided with the request.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        AccountMerge_SqlServer? accountMergeRecord =
            await _accountMergeManager.GetRecordAsync((int)accountMergeId);
        // Edge case #2
        // If the AccountMerge record is not found.
        // Redirect to the Manage Account page.
        if (accountMergeRecord == null)
        {
            _logger.LogWarning(
                "The AccountMerge record {AccountMergeId} was not found.",
                accountMergeId
            );
            StatusMessage = "The AccountMerge record was not found.";
            return RedirectToPage("/Account/Manage/Index");
        }

        // Edge case #3
        // If the current user is not one of the users participating in the merge request. 
        // It violates the workflow rules.
        // Redirect to the Manage Account page, and tell the record was not found.
        if (
            accountMergeRecord.PrimaryUserId != CurrentUserId
            && accountMergeRecord.SecondaryUserId != CurrentUserId
        )
        {
            _logger.LogWarning(
                "User {UserId} requested the AccountMerge {AccountMergeId}, " +
                    "in which they are not one of the users taking part.",
                CurrentUserId,
                accountMergeId
            );
            StatusMessage = "The AccountMerge record was not found.";
            return RedirectToPage("/Account/Manage/Index");
        }

        AccountMergeRecord = accountMergeRecord;

        PrimaryUser = await _userManager.FindByIdAsync(accountMergeRecord.PrimaryUserId);
        SecondaryUser = await _userManager.FindByIdAsync(accountMergeRecord.SecondaryUserId);
        // Edge case #4
        // If one of the users cannot be found.
        // Redirect to AccountMergeList page.
        if (
            PrimaryUser == null
            || SecondaryUser == null)
        {
            _logger.LogError("One of the users participating in the AccountMerge was not " +
                "found. Users {PrimaryUserId} and {SecondaryUserId}.",
                accountMergeRecord.PrimaryUserId,
                accountMergeRecord.SecondaryUserId
            );
            StatusMessage = "One of the users participating in the AccountMerge was not " +
                "found. Please return later.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        PrimaryUser_CurrentLogins = await _userManager.GetLoginsAsync(PrimaryUser);
        SecondaryUser_CurrentLogins = await _userManager.GetLoginsAsync(SecondaryUser);

        return Page();
    }



    public async Task<IActionResult> OnPostConfirmAsync(
        [FromForm] int? accountMergeId,
        [FromForm(Name = "Timestamp")] string? accountMergeTimestamp
    )
    {
        string userId = _userManager.GetUserId(User);

        // Edge case #1
        // If the AccountMergeId was not provided.
        // Redirect to the Account Merge List page.
        if (accountMergeId == null)
        {
            _logger.LogWarning(
                "AccountMergeId parameter wasn't provided in method {MethodName}.",
                nameof(OnPostConfirmAsync)
            );
            StatusMessage = "AccountMergeId parameter wasn't provided with the request.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #2
        // If the Timestamp not provided.
        // Tell the user the parameter was not provided.
        if (accountMergeTimestamp == null)
        {
            _logger.LogWarning(
                "Timestamp parameter wasn't provided in the method {MethodName}.",
                nameof(OnPostRejectAsync)
            );
            StatusMessage = "Timestamp parameter wasn't provided with the request.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }
        byte[] _accountMergeTimestamp = Convert.FromBase64String(accountMergeTimestamp);

        OneOf<AccountMergeSuccess, AccountMergeConfirmed, EntityNotFound, ConcurrencyConflict,
            WorkflowRulesViolation> confirmMergeResult =
            await _accountMergeManager.ConfirmAsync(
                (int)accountMergeId,
                userId,
                _accountMergeTimestamp
            );

        // Edge case #3
        // If the AccountMerge record was not found.
        // Redirect to the AccountMergeList page.
        if (confirmMergeResult.IsT2)
        {
            EntityNotFound entityNotFound = confirmMergeResult.AsT2;
            _logger.LogWarning(
                "The AccountMerge record {AccountMergeId} was not found.",
                accountMergeId
            );
            StatusMessage = "The AccountMerge record was not found.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #4
        // If there was a concurrency conflict during the AccountMerge confirmation.
        // Tell the user about the conflict and ask to try again, redirect to the AccountMergeList
        // page.
        if (confirmMergeResult.IsT3)
        {
            ConcurrencyConflict concurrencyConflict = confirmMergeResult.AsT3;
            StatusMessage = "There was a concurrency conflict modifying the record. " +
                "Please review the data and try again.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #5
        // If there was some workflow rules violation.
        // Tell the user about an error and redirect to the AccountMergeList page.
        if (confirmMergeResult.IsT4)
        {
            WorkflowRulesViolation workflowRulesViolation = confirmMergeResult.AsT4;
            StatusMessage = "There was an error. Please try again later.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        if (confirmMergeResult.IsT1)
        {
            AccountMergeConfirmed confirmedResult = confirmMergeResult.AsT1;
            _logger.LogInformation(
                "AccountMerge {AccountMergeId} was confirmed by User {UserId}.",
                confirmedResult.AccountMerge.Id,
                userId
            );
            StatusMessage = "The AccountMerge was confirmed for this account. " +
                "To finish the merge process, please log-in into the second account " +
                "and confirm the AccountMerge from there.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #6
        // If the ConfirmMergeResult type is not one of the expected ones.
        // Tell the user about an error and redirect to the AccountMergeList page.
        if (confirmMergeResult.IsT0 == false)
        {
            _logger.LogError(
                "Unexpected CancelResult type {CancelResultType} returned.",
                confirmMergeResult.Value.GetType()
            );
            StatusMessage = "The AccountMerge was not confirmed because of our error. " +
                "Please try again later.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        AccountMergeSuccess mergedResult = confirmMergeResult.AsT0;
        _logger.LogInformation(
            "AccountMerge {AccountMergeId} was merged after confirmation by User {UserId}.",
            mergedResult.AccountMerge.Id,
            userId
        );

        if (mergedResult.AccountMerge.SecondaryUserId == userId)
        {
            // Log-out the current user from the browser, as they were deleted from the database:
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        }

        StatusMessage = "The accounts were merged successfully.";
        return RedirectToPage("/Account/Manage/Index");
    }



    public async Task<IActionResult> OnPostRejectAsync(
        [FromForm] int? accountMergeId,
        [FromForm(Name = "Timestamp")] string? accountMergeTimestamp
    )
    {
        string userId = _userManager.GetUserId(User);

        // Edge case #1
        // If the AccountMergeId was not provided.
        // Redirect to the Account Merge List page.
        if (accountMergeId == null)
        {
            _logger.LogWarning(
                "AccountMergeId parameter wasn't provided in the method {MethodName}.",
                nameof(OnPostRejectAsync)
            );
            StatusMessage = "AccountMergeId parameter wasn't provided with the request.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #2
        // If the Timestamp not provided.
        // Tell the user the parameter was not provided.
        if (accountMergeTimestamp == null)
        {
            _logger.LogWarning(
                "Timestamp parameter wasn't provided in the method {MethodName}.",
                nameof(OnPostRejectAsync)
            );
            StatusMessage = "Timestamp parameter wasn't provided with the request.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }
        byte[] _accountMergeTimestamp = Convert.FromBase64String(accountMergeTimestamp);

        OneOf<AccountMerge_SqlServer, EntityNotFound, ConcurrencyConflict, WorkflowRulesViolation>
            rejectResult = await _accountMergeManager.RejectAsync(
                (int)accountMergeId,
                userId,
                _accountMergeTimestamp
            );

        // Edge case #3
        // If the AccountMerge record was not found.
        // Redirect to the AccountMergeList page.
        if (rejectResult.IsT1)
        {
            _logger.LogWarning(
                "The AccountMerge record {AccountMergeId} was not found, " +
                    "from User {UserId} request.",
                accountMergeId,
                userId
            );
            EntityNotFound entityNotFound = rejectResult.AsT1;
            StatusMessage = "The AccountMerge record was not found.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #4
        // If there was a concurrency conflict during the AccountMerge record delete.
        // Tell the user about a conflict and ask to try again, redirect to the AccountMergeList
        // page.
        if (rejectResult.IsT2)
        {
            ConcurrencyConflict concurrencyConflict = rejectResult.AsT2;
            StatusMessage = "There was a concurrency conflict modifying the record. " +
                "Please review the data and try again.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #5
        // If there was some workflow rules violation.
        // Tell the user about an error and redirect to the AccountMergeList page.
        if (rejectResult.IsT3)
        {
            WorkflowRulesViolation workflowRulesViolation = rejectResult.AsT3;
            StatusMessage = "There was an error. Please try again later.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        // Edge case #6
        // If the CancelResult type is not one of the expected ones.
        // Tell the user about an error and redirect to the AccountMergeList page.
        if (rejectResult.IsT0 == false)
        {
            _logger.LogError(
                "Unexpected CancelResult type {CancelResultType} returned.",
                rejectResult.Value.GetType()
            );
            StatusMessage = "The AccountMerge request was not canceled because of our " +
                "error. Please try again later.";
            return RedirectToPage("/Account/Manage/AccountMergeList");
        }

        AccountMerge_SqlServer success = rejectResult.AsT0;
        _logger.LogInformation(
            "User {UserId} rejected the AccountMerge request {AccountMergeId}.",
            userId,
            accountMergeId
        );
        StatusMessage = "The AccountMerge request was canceled successfully.";
        return RedirectToPage("/Account/Manage/AccountMergeList");
    }
}