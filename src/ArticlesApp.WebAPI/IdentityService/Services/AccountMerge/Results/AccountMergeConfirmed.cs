using ArticlesApp.Database.SqlServer.Models;



namespace ArticlesApp.WebAPI.IdentityService.Services.AccountMerge.Results;



public class AccountMergeConfirmed
{
    public AccountMergeConfirmed(AccountMerge_SqlServer accountMerge)
    {
        AccountMerge = accountMerge;
    }



    public AccountMerge_SqlServer AccountMerge { get; }
}