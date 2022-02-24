namespace ArticlesApp.WebAPI._AppConfiguration.Sections.UserAccounts;



public class UserAccountsConfigurationBuilder
{
    public static UserAccountsConfiguration Build(IConfiguration hostConfig)
    {
        return new UserAccountsConfiguration
        {
            EnableLocalAccounts = hostConfig.GetValue<bool?>("Custom:IdentityServer:EnableLocalAccounts") ?? false,
            SoftDeletionPeriodDays = hostConfig.GetValue<int?>("Custom:SoftDeletionPeriodDays") ?? 7
        };
    }
}