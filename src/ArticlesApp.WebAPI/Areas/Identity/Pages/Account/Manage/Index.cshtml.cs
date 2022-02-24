using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public partial class IndexModel : PageModel
{
    private readonly SignInManager<ApplicationUser_DB> _signInManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;

    public IndexModel(
        SignInManager<ApplicationUser_DB> signInManager,
        UserManager<ApplicationUser_DB> userManager
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }



    [BindProperty]
    public InputModel? Input { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public string? Username { get; set; }



    public class InputModel
    {
        [Required]
        [RegularExpression(
            "^[a-zA-Z0-9\\.@\\-_ ]*$",
            ErrorMessage =
                "Username allowed characters are a-z, A-Z, 0-9, . , @ , " +
                "- , _ , &#160&#160&#160(space)."
        )]
        [StringLength(
            256,
            MinimumLength = 4,
            ErrorMessage = "Username length should be between 4 and 256 characters."
        )]
        [Display(Name = "New username")]
        public string? UserName { get; set; }
    }



    public async Task<IActionResult> OnGetAsync()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login");
        }

        await LoadAsync(user);

        return Page();
    }



    public async Task<IActionResult> OnPostAsync()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login");
        }

        if (ModelState.IsValid == false)
        {
            await LoadAsync(user);

            return Page();
        }

        await _userManager.SetUserNameAsync(user, Input!.UserName);

        await _signInManager.RefreshSignInAsync(user);

        StatusMessage = "Your profile was updated.";
        return RedirectToPage();
    }



    private async Task LoadAsync(ApplicationUser_DB user)
    {
        string username = await _userManager.GetUserNameAsync(user);

        Username = username;
        Input = new InputModel
        {
            UserName = username
        };
    }
}