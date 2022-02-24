namespace ArticlesApp.WebAPI.Areas.Identity.Pages.Account;



public static class Routes
{
    private static readonly string _relativePart = "./";
    private static readonly string _absolutePart = "/Identity/Account/";
    //private static readonly string _absoluteManagePart = "/Identity/Account/Manage";

    private static readonly string _accountRecoveryPage = "AccountRecovery";
    public static string AccountRecovery_Absolute => _absolutePart + _accountRecoveryPage;
    public static string AccountRecovery_Relative => _relativePart + _accountRecoveryPage;
}