using ArticlesApp.Core.Entities.PersonalData;
using ArticlesApp.Core.Entities.SoftDeletion;
using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Models.SoftDeletion;
using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer.Models;
using Microsoft.EntityFrameworkCore;



namespace ArticlesApp.Database.SqlServer.Repositories;



public class UserRepository_SqlServer : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository_SqlServer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }



    public async Task SoftDeleteUserByIdAsync(
        string userId,
        DateTime nowUtc
    )
    {
        DateTime softDeletedAt_DateUtc = nowUtc.ToUniversalTime();

        ApplicationUser_DB user = await _dbContext.Users.FindAsync(userId)
            ?? throw new EntityNotFoundException();

        _dbContext.Entry<ApplicationUser_DB>(user!)
            .Property(u => u.SoftDeletedAt_DateUtc)
            .CurrentValue = softDeletedAt_DateUtc;

        Article_SqlServer[] articles = await _dbContext.Articles!
            .Where(ar => ar.AuthorId == userId)
            .Where(ar => ar.SoftDeletionReason_ReasonId == null)
            .ToArrayAsync();

        foreach (Article_SqlServer ar in articles)
        {
            _dbContext.Entry(ar)
                .Property(a => a.SoftDeletedAt_DateUtc)
                .CurrentValue = softDeletedAt_DateUtc;

            _dbContext.Entry(ar)
                .Property(a => a.SoftDeletionReason_ReasonId)
                .CurrentValue = SoftDeletionReasonEnum.UserSoftDeleted;
        }

        await _dbContext.SaveChangesAsync();
    }



    public async Task<bool> IsSoftDeletedAsync(string userId)
    {
        ApplicationUser_DB user = await _dbContext.Users.FindAsync(userId)
            ?? throw new EntityNotFoundException();

        if (user.SoftDeletedAt_DateUtc != null)
        {
            return true;
        }

        return false;
    }



    public async Task ReinstateUserByIdAsync(string userId)
    {
        ApplicationUser_DB user = await _dbContext.Users
            .FindAsync(userId)
            ?? throw new EntityNotFoundException();

        _dbContext.Entry(user)
            .Property(u => u.SoftDeletedAt_DateUtc)
            .CurrentValue = null;

        Article_SqlServer[] articles = await _dbContext.Articles
            .Where(ar => ar.AuthorId == user.Id)
            .Where(ar => ar.SoftDeletionReason_ReasonId == SoftDeletionReasonEnum.UserSoftDeleted)
            .ToArrayAsync();

        foreach (Article_SqlServer article in articles)
        {
            _dbContext.Entry(article)
                .Property(ar => ar.SoftDeletionReason_ReasonId)
                .CurrentValue = null;

            _dbContext.Entry(article)
                .Property(ar => ar.SoftDeletedAt_DateUtc)
                .CurrentValue = null;
        }

        await _dbContext.SaveChangesAsync();
    }



    public async Task<HardDeletedUsersCount> HardDeleteUsersAsync(DateTime olderThan_DateUtc)
    {
        ApplicationUser_DB[] users = await _dbContext.Users
            .Where(user => user.SoftDeletedAt_DateUtc < olderThan_DateUtc)
            .ToArrayAsync();

        int articlesCount = 0;
        foreach (ApplicationUser_DB user in users)
        {
            int articles = await HardDeleteUsersArticlesAsync(user);
            articlesCount += articles;
        }
        _dbContext.RemoveRange(users);
        await _dbContext.SaveChangesAsync();

        return new HardDeletedUsersCount()
        {
            Users = users.Length,
            Articles = articlesCount
        };
    }

    private async Task<int> HardDeleteUsersArticlesAsync(ApplicationUser_DB user)
    {
        Article_SqlServer[] articles = await _dbContext.Articles
            .Where(ar => ar.AuthorId == user.Id)
            .ToArrayAsync();

        _dbContext.RemoveRange(articles);

        return articles.Length;
    }



    public async Task<PersonalData_UserInfo> UserDownloadsPersonalDataAsync(string userId)
    {
        ApplicationUser_DB user = await _dbContext.Users
            .Where(usr => usr.Id == userId)
            .FirstOrDefaultAsync()
            ?? throw new EntityNotFoundException();

        return new PersonalData_UserInfo(
            id: user.Id,
            userName: user.UserName,
            email: user.Email
        );
    }



    public async Task ChangeEmailConfirmationToFalseAsync(string userId)
    {
        ApplicationUser_DB user = await _dbContext.Users
            .Where(usr => usr.Id == userId)
            .FirstOrDefaultAsync()
            ?? throw new EntityNotFoundException();

        _dbContext.Entry(user)
            .Property(usr => usr.EmailConfirmed)
            .CurrentValue = false;

        await _dbContext.SaveChangesAsync();
    }
}