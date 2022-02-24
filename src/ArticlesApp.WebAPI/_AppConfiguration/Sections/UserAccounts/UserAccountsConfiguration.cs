namespace ArticlesApp.WebAPI._AppConfiguration.Sections.UserAccounts;



public class UserAccountsConfiguration
{
    public bool EnableLocalAccounts { get; init; }
    public int SoftDeletionPeriodDays { get; init; }
}