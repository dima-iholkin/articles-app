using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer.Models;
using ArticlesApp.WebAPI.IdentityService.Services.AccountMerge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public class AccountMergeListModel : PageModel
{
    private readonly AccountMergeManager _accountMergeManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;

    public AccountMergeListModel(
        AccountMergeManager accountMergeManager,
        UserManager<ApplicationUser_DB> userManager
    )
    {
        _accountMergeManager = accountMergeManager;
        _userManager = userManager;
    }



    public List<AccountMerge_SqlServer> AccountMerges { get; set; } = new();

    public ApplicationUser_DB? CurrentUser { get; set; }

    public Dictionary<string, ApplicationUser_DB> OtherUsers { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }



    public async Task<IActionResult> OnGetAsync()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        // Edge case #1
        // If the user is not found.
        // Redirect to the Login page.
        if (CurrentUser == null)
        {
            return RedirectToPage("/Account/Login");
        }

        List<AccountMerge_SqlServer> _accountMerges = 
            await _accountMergeManager.GetRecordsAsync(CurrentUser.Id);

        // Order the records, first by the primary account being the current user:
        IEnumerable<AccountMerge_SqlServer> recordsWhereCurrentUserIsPrimary = 
            _accountMerges.Where(acc => acc.PrimaryUserId == CurrentUser.Id);
        IEnumerable<AccountMerge_SqlServer> recordsWhereCurrentUserIsSecondary = 
            _accountMerges.Where(acc => acc.PrimaryUserId != CurrentUser.Id);
        AccountMerges.AddRange(recordsWhereCurrentUserIsPrimary);
        AccountMerges.AddRange(recordsWhereCurrentUserIsSecondary);

        // Fill the OtherUsers dictionary for the Page view:
        foreach (AccountMerge_SqlServer acc in AccountMerges)
        {
            string otherUserId;
            if (acc.PrimaryUserId != CurrentUser.Id)
            {
                otherUserId = acc.PrimaryUserId;
            } else
            {
                otherUserId = acc.SecondaryUserId;
            }

            ApplicationUser_DB? otherUser = await _userManager.FindByIdAsync(otherUserId);
            if (otherUser != null)
            {
                OtherUsers.Add(
                    otherUserId, 
                    otherUser
                );
            }
        }

        return Page();
    }
}