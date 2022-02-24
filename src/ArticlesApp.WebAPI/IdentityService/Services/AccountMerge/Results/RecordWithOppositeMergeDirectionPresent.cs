namespace ArticlesApp.WebAPI.IdentityService.Services.AccountMerge.Results;



public class RecordWithOppositeMergeDirectionPresent
{
    public RecordWithOppositeMergeDirectionPresent(int accountsMergeId)
    {
        AccountMergeId = accountsMergeId;
    }



    public int AccountMergeId { get; set; }
}