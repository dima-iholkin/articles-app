using System.Reflection;
using System.Text.Json;
using ArticlesApp.Core.Entities.PersonalData;
using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Database.Models;
using ArticlesApp.WebAPI.IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account.Manage;



[Authorize]
public class DownloadPersonalDataModel : PageModel
{
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly UsersOrchestrator _usersOrchestrator;
    private readonly ILogger<DownloadPersonalDataModel> _logger;

    public DownloadPersonalDataModel(
        UserManager<ApplicationUser_DB> userManager,
        UsersOrchestrator usersOrchestrator,
        ILogger<DownloadPersonalDataModel> logger
    )
    {
        _userManager = userManager;
        _usersOrchestrator = usersOrchestrator;
        _logger = logger;
    }



    public async Task<IActionResult> OnPostAsync()
    {
        ApplicationUser_DB? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("Unable to find the user {UserId}.", _userManager.GetUserId(User));

            return NotFound($"Unable to find the user.");
        }

        _logger.LogInformation("User '{UserId}' asked to download their personal data.", _userManager.GetUserId(User));

        // Includes only the personal data into the userInfo download:
        Dictionary<string, string> userInfo = new Dictionary<string, string>();
        IEnumerable<PropertyInfo> personalDataProperties =
            typeof(ApplicationUser_DB).GetProperties()
                .Where(prop =>
                    Attribute.IsDefined(prop, typeof(PersonalDataAttribute))
                );
        foreach (PropertyInfo p in personalDataProperties)
        {
            userInfo.Add(
                p.Name,
                p.GetValue(user)?.ToString() ?? "null"
            );
        }

        // Gets the logins info into the userInfo download:
        IList<UserLoginInfo> logins = await _userManager.GetLoginsAsync(user);
        foreach (UserLoginInfo l in logins)
        {
            userInfo.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        PersonalData personalData = await _usersOrchestrator.UserDownloadsPersonalDataAsync(user.Id);
        PersonalData_WebAPI personalData_WebAPI = new PersonalData_WebAPI(
             personalData.Articles,
             userInfo
        );

        Response.Headers.Add(
            "Content-Disposition",
            "attachment; filename=PersonalData.json"
        );
        return new FileContentResult(
            JsonSerializer.SerializeToUtf8Bytes(personalData_WebAPI),
            "application/json"
        );
    }
}