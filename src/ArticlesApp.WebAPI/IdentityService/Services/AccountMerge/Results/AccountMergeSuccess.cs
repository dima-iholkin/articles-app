using ArticlesApp.Database.SqlServer.Models;



namespace ArticlesApp.WebAPI.IdentityService.Services.AccountMerge.Results;



public class AccountMergeSuccess
{
    public AccountMergeSuccess(AccountMerge_SqlServer accountMerge)
    {
        AccountMerge = accountMerge;
    }



    public AccountMerge_SqlServer AccountMerge { get; }
}