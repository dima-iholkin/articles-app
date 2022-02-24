using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer;
using ArticlesApp.Database.SqlServer.Models;
using ArticlesApp.WebAPI.IdentityService.Services.AccountMerge.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;



namespace ArticlesApp.WebAPI.IdentityService.Services.AccountMerge;



public class AccountMergeManager
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AccountMergeManager> _logger;
    private readonly UserManager<ApplicationUser_DB> _userManager;

    public AccountMergeManager(
        ApplicationDbContext dbContext,
        ILogger<AccountMergeManager> logger,
        UserManager<ApplicationUser_DB> userManager
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
    }



    public async Task<OneOf<AccountMergeSuccess, AccountMergeConfirmed, EntityNotFound,
        ConcurrencyConflict, WorkflowRulesViolation>>
        ConfirmAsync
    (
        int accountMergeId,
        string currentUserId,
        byte[] accountMergeTimestamp
    )
    {
        AccountMerge_SqlServer? accountMergeRecord =
            await _dbContext.AccountMerges.FindAsync(accountMergeId);
        // Edge case #1
        // If the AccountMerges record was not found.
        // Return the EntityNotFound result.
        if (accountMergeRecord == null)
        {
            return new EntityNotFound();
        }

        // Edge case #2
        // If the Timestamp was not provided.
        // Return the WorkflowRulesViolation result.
        if (
            accountMergeTimestamp == null
            || accountMergeTimestamp.Length == 0
        )
        {
            return new WorkflowRulesViolation();
        }

        // Edge case #3
        // If the current user is not one of the user accounts involved in the AccountMerge.
        // It violates the workflow rules.
        // Return the EntityNotFound result, for the privacy reasons.
        if (
            accountMergeRecord.PrimaryUserId != currentUserId
            && accountMergeRecord.SecondaryUserId != currentUserId
        )
        {
            return new EntityNotFound();
        }

        // Confirm the AccountMerge for the current user:
        if (currentUserId == accountMergeRecord.PrimaryUserId)
        {
            accountMergeRecord.PrimaryUserConfirmed = true;
        }
        else if (currentUserId == accountMergeRecord.SecondaryUserId)
        {
            accountMergeRecord.SecondaryUserConfirmed = true;
        }

        // If both users have not confirmed the merge.
        // Return the confirmation result only.
        if (
            accountMergeRecord.PrimaryUserConfirmed == false
            || accountMergeRecord.SecondaryUserConfirmed == false
        )
        {
            // Add the concurrency check:
            _dbContext.Entry(accountMergeRecord)
                .Property(acc => acc.Timestamp)
                .OriginalValue = accountMergeTimestamp;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Edge case #4
                // If there is a concurrency conflict during the AccountMerge confirmation.
                // Return the ConcurrencyConflict result.
                _logger.LogInformation(
                    "Concurrency conflict during method {MethodName} call for AccountMerge " +
                    "record {AccountMergeId}.",
                    nameof(ConfirmAsync),
                    accountMergeId
                );
                return new ConcurrencyConflict();
            }

            return new AccountMergeConfirmed(accountMergeRecord);
        }

        await ReassignEntitiesToPrimaryUserAsync(
                accountMergeRecord.PrimaryUserId,
                accountMergeRecord.SecondaryUserId
            );
        await DeleteSecondaryUserAccountMergesAsync(accountMergeRecord.SecondaryUserId);
        await DeleteSecondaryUserAsync(accountMergeRecord.SecondaryUserId);

        return new AccountMergeSuccess(accountMergeRecord);
    }



    public async
        Task<OneOf<AccountMerge_SqlServer, RecordWithOppositeMergeDirectionPresent, WorkflowRulesViolation>>
        CreateAsync
    (
        string primaryUserId,
        string secondaryUserId
    )
    {
        // Edge case #1
        // If the provided 2 userIds are the same. It violates the workflow rules.
        // Return the WorkflowRulesViolation result.
        if (primaryUserId == secondaryUserId)
        {
            _logger.LogError(
                "Cannot merge a pair of users with the same UserId {UserId}.",
                primaryUserId
            );
            return new WorkflowRulesViolation();
        }

        List<AccountMerge_SqlServer> accountMerges = await _dbContext.AccountMerges
            .Where(acc =>
                (acc.PrimaryUserId == primaryUserId && acc.SecondaryUserId == secondaryUserId)
                || (
                    acc.PrimaryUserId == secondaryUserId
                    && acc.SecondaryUserId == primaryUserId
                )
            )
            .ToListAsync();

        // Edge case #2
        // If a record is present for this pair of users, with the right merge direction.
        // Return the present record.
        AccountMerge_SqlServer? presentRecord = accountMerges
            .Where(acc =>
                acc.PrimaryUserId == primaryUserId && acc.SecondaryUserId == secondaryUserId
            )
            .OrderBy(acc => acc.Id)
            .FirstOrDefault();
        if (presentRecord != null)
        {
            _logger.LogInformation(
                "AccountMerge record is present already " +
                    "for users {FirstUserId} and {SecondUserId}.",
                primaryUserId,
                secondaryUserId
            );
            return presentRecord;
        }

        // Edge case #3
        // If a record is present for this pair of users, BUT with a WRONG merge direction.
        // Return the RecordWithOppositeMergeDirectionPresent result.
        AccountMerge_SqlServer? recordWithWrongMergeDirection = accountMerges
            .Where(acc =>
                acc.PrimaryUserId == secondaryUserId && acc.SecondaryUserId == primaryUserId
            )
            .OrderBy(acc => acc.Id)
            .FirstOrDefault();
        if (recordWithWrongMergeDirection != null)
        {
            _logger.LogInformation(
                "AccountMerge record with the WRONG merge direction is present already " +
                    "for users {FirstUserId} and {SecondUserId}.",
                primaryUserId,
                secondaryUserId
            );
            return new RecordWithOppositeMergeDirectionPresent(
                recordWithWrongMergeDirection.Id
            );
        }

        AccountMerge_SqlServer newRecord = new AccountMerge_SqlServer(
            primaryUserId,
            secondaryUserId
        );
        _dbContext.AccountMerges.Add(newRecord);
        await _dbContext.SaveChangesAsync();

        return newRecord;

        // During the creation, we don't care about concurrently creating the duplicate records,
        // if we always everywhere choose the AccountMerge record with the lowest Id,
        // and delete all the other AccountMerge records during the merge.
    }



    public async Task<AccountMerge_SqlServer?> GetRecordAsync(int accountMergeId)
    {
        AccountMerge_SqlServer? accountMergeRecord =
            await _dbContext.AccountMerges.FindAsync(accountMergeId);
        return accountMergeRecord;
    }



    public async Task<List<AccountMerge_SqlServer>> GetRecordsAsync(string userId)
    {
        List<AccountMerge_SqlServer> accountMergeRecords = await _dbContext.AccountMerges
            .Where(acc =>
                acc.PrimaryUserId == userId
                || acc.SecondaryUserId == userId
            )
            .OrderBy(acc => acc.Id)
            .ToListAsync();

        return accountMergeRecords;
    }



    public async
        Task<OneOf<AccountMerge_SqlServer, EntityNotFound, ConcurrencyConflict, WorkflowRulesViolation>>
        RejectAsync
    (
        int accountMergeId,
        string currentUserId,
        byte[] accountMergeTimestamp
    )
    {
        AccountMerge_SqlServer? accountMergeRecord =
            await _dbContext.AccountMerges.FindAsync(accountMergeId);
        // Edge case #1
        // If the AccountMerges record was not found.
        // Return the EntityNotFound result.
        if (accountMergeRecord == null)
        {
            return new EntityNotFound();
        }

        // Edge case #2
        // If the Timestamp was not provided.
        // Return the WorkflowRulesViolation result.
        if (
            accountMergeTimestamp == null
            || accountMergeTimestamp.Length == 0
        )
        {
            return new WorkflowRulesViolation();
        }

        // Edge case #3
        // If the current user is not one of the user accounts involved in the AccountMerge.
        // It violates the workflow rules.
        // Return the EntityNotFound result, for the privacy reasons.
        if (
            accountMergeRecord.PrimaryUserId != currentUserId
            && accountMergeRecord.SecondaryUserId != currentUserId
        )
        {
            return new EntityNotFound();
        }

        // Add the concurrency check:
        _dbContext.Entry(accountMergeRecord)
            .Property(acc => acc.Timestamp)
            .OriginalValue = accountMergeTimestamp;

        _dbContext.AccountMerges.Remove(accountMergeRecord);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Edge case #4
            // If there is a concurrency conflict during the AccountMerge record delete.
            // Return the ConcurrencyConflict result.
            _logger.LogInformation(
                "Concurrency conflict during method {MethodName} call for AccountMerge record " +
                    "{AccountMergeId}.",
                nameof(RejectAsync),
                accountMergeId
            );
            return new ConcurrencyConflict();
        }

        return accountMergeRecord;
    }



    private async Task DeleteSecondaryUserAccountMergesAsync(string secondaryUserId)
    {
        List<AccountMerge_SqlServer>? secondaryUserRecords = await _dbContext.AccountMerges
            .Where(acc =>
                acc.PrimaryUserId == secondaryUserId
                || acc.SecondaryUserId == secondaryUserId
            )
            .ToListAsync();
        _dbContext.AccountMerges.RemoveRange(secondaryUserRecords);

        await _dbContext.SaveChangesAsync();
    }



    private async Task DeleteSecondaryUserAsync(string secondaryUserId)
    {
        ApplicationUser_DB secondUser = await _userManager.FindByIdAsync(secondaryUserId);
        await _userManager.DeleteAsync(secondUser);
    }



    private async Task ReassignEntitiesToPrimaryUserAsync(
        string primaryUserId,
        string secondaryUserId
    )
    {
        List<IdentityUserLogin<string>> secondUserExternalLogins = await _dbContext.UserLogins
            .Where(_login => _login.UserId == secondaryUserId)
            .ToListAsync();

        // Reassign the secondary user's ExternalLogins to the primary user:
        foreach (IdentityUserLogin<string> login in secondUserExternalLogins)
        {
            _dbContext.Entry<IdentityUserLogin<string>>(login)
                .Property(_login => _login.UserId)
                .CurrentValue = primaryUserId;
        }

        // Reassign the secondary user's Articles to the primary user:
        List<Article_SqlServer> articles = await _dbContext.Articles
            .Where(ar => ar.AuthorId == secondaryUserId)
            .ToListAsync();
        foreach (Article_SqlServer article in articles)
        {
            _dbContext.Entry<Article_SqlServer>(article)
                .Property(ar => ar.AuthorId)
                .CurrentValue = primaryUserId;
        }

        // Reassign the secondary user's Notifications to the primary user:
        List<Notification_SqlServer> notifications = await _dbContext.Notifications
            .Where(notif => notif.Reciever_UserId == secondaryUserId)
            .ToListAsync();
        foreach (Notification_SqlServer notification in notifications)
        {
            // Remove the secondary user's Notification:
            _dbContext.Remove<Notification_SqlServer>(notification);

            // Recreate the Notification for the primary user:
            Notification_SqlServer newNotification = new Notification_SqlServer(
                0,
                notification.Message,
                primaryUserId,
                notification.CreatedAt_DateUtc
            )
            {
                ReferencedArticle_ArticleId = notification.ReferencedArticle_ArticleId,
                NotificationType_TypeId = notification.NotificationType_TypeId,
                ReadAt_DateUtc = notification.ReadAt_DateUtc,
            };
            _dbContext.Notifications.Add(newNotification);
        }

        await _dbContext.SaveChangesAsync();
    }
}