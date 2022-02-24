using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Identity;
using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Entities.SoftDeletion;
using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Identity;



namespace ArticlesApp.Database.SqlServer.Dev_DbInitializer.LookupTables;



public static class LookupTablesInitializer
{
    public static void InitializeDb(
        RoleManager<ApplicationUserRole_DB> roleManager,
        ApplicationDbContext dbContext
    )
    {
        InitializeUserRoles(roleManager);

        InitializeArticleStates(dbContext);

        InitializeNotificationTypes(dbContext);

        InitializeSoftDeletionReasons(dbContext);
    }



    private static void InitializeUserRoles(RoleManager<ApplicationUserRole_DB> roleManager)
    {
        if (roleManager.Roles.Any())
        {
            throw new Exception("Expected no user roles in the DB table before initialization.");
        }

        foreach (IdentityRolesEnum roleEnum in Enum.GetValues<IdentityRolesEnum>())
        {
            string roleName = Enum.GetName<IdentityRolesEnum>(roleEnum)!;
            ApplicationUserRole_DB role = new ApplicationUserRole_DB(roleName);

            roleManager.CreateAsync(role).Wait();
        }
        // Populate the DB with the possible user roles.
    }



    private static void InitializeArticleStates(ApplicationDbContext dbContext)
    {
        if (dbContext.ArticleStates.Any())
        {
            throw new Exception("Expected no article states in the DB before initialization.");
        }

        IList<ArticleState> states = new List<ArticleState>();
        foreach (ArticleStatesEnum articleStateId in Enum.GetValues<ArticleStatesEnum>())
        {
            ArticleState st = new ArticleState
            {
                Id = articleStateId,
                Name = articleStateId.ToString()
            };
            states.Add(st);
        }

        dbContext.ArticleStates.AddRange(states);
        dbContext.SaveChanges();
        // Populate the DB with the possible article states.
    }



    private static void InitializeNotificationTypes(ApplicationDbContext dbContext)
    {
        if (dbContext.NotificationTypes.Any())
        {
            throw new Exception("Expected no notification types in the DB before initialization.");
        }

        IList<NotificationType> types = new List<NotificationType>();
        foreach (NotificationTypesEnum type in Enum.GetValues<NotificationTypesEnum>())
        {
            NotificationType tp = new NotificationType
            {
                Id = type,
                Name = Enum.GetName(type)!
            };
            types.Add(tp);
        }

        dbContext.NotificationTypes.AddRange(types);
        dbContext.SaveChanges();
        // populate the DB with the possible notification types.
    }



    private static void InitializeSoftDeletionReasons(ApplicationDbContext dbContext)
    {
        if (dbContext.SoftDeletionReasons.Any())
        {
            throw new Exception("Expected no records in this table in the DB before initialization.");
        }

        List<SoftDeletionReason> reasons = new List<SoftDeletionReason>();
        foreach (SoftDeletionReasonEnum reasonEnum in Enum.GetValues<SoftDeletionReasonEnum>())
        {
            SoftDeletionReason reason = new SoftDeletionReason()
            {
                Id = reasonEnum,
                Name = reasonEnum.ToString()
            };
            reasons.Add(reason);
        }

        dbContext.SoftDeletionReasons.AddRange(reasons);
        dbContext.SaveChanges();
    }
}