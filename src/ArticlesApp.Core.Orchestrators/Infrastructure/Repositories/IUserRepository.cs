using ArticlesApp.Core.Entities.PersonalData;
using ArticlesApp.Core.Orchestrators.Models.SoftDeletion;



namespace ArticlesApp.Core.Orchestrators.Infrastructure;



public interface IUserRepository
{
    public Task SoftDeleteUserByIdAsync(
        string userId,
        DateTime nowUtc
    );

    public Task<bool> IsSoftDeletedAsync(string userId);

    public Task ReinstateUserByIdAsync(string userId);

    public Task ChangeEmailConfirmationToFalseAsync(string userId);

    public Task<PersonalData_UserInfo> UserDownloadsPersonalDataAsync(string userId);

    public Task<HardDeletedUsersCount> HardDeleteUsersAsync(DateTime olderThan_DateUtc);
}