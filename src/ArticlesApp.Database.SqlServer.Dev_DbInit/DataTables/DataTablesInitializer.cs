using System.Reflection;
using System.Resources;
using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Identity;
using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;



namespace ArticlesApp.Database.SqlServer.Dev_DbInitializer.DataTables;



public static class DataTablesInitializer
{
    public static void InitializeDb_Dev(
        UserManager<ApplicationUser_DB> userManager,
        ApplicationDbContext dbContext,
        UsersOrchestrator usersOrchestrator,
        IConfiguration hostConfiguration
    )
    {
        ApplicationUser_DB userA = InitializeUsers(
            userManager,
            usersOrchestrator,
            hostConfiguration
        );

        InitializeArticles(
            userA,
            dbContext
        );
    }



    private static Article_SqlServer[] InitializeArticles(
        ApplicationUser_DB userA,
        ApplicationDbContext dbContext
    )
    {
        ResourceManager rm = new ResourceManager(
            "ArticlesApp.Database.SqlServer.Dev_DbInit.DataTables.Resource1",
            Assembly.GetExecutingAssembly()
        );
        string gettingStartedGuideText = rm.GetString("GettingStartedGuideText")
            ?? throw new Exception("Article's text was not found in the resource file.");

        Article_SqlServer[] articlesArray = new Article_SqlServer[] {
            new Article_SqlServer(
                id: 0,
                title: "Hello! Getting started guide",
                text: gettingStartedGuideText,
                authorId: userA.Id,
                submittedAt_DateUtc: DateTime.Now.ToUniversalTime(),
                articleStateId: ArticleStatesEnum.Approved,
                versionId: 0
            )
        };

        dbContext.AddRange(articlesArray);
        dbContext.SaveChanges();

        return articlesArray;
    }



    private static ApplicationUser_DB InitializeUsers(
        UserManager<ApplicationUser_DB> userManager,
        UsersOrchestrator usersOrchestrator,
        IConfiguration hostConfiguration
    )
    {
        if (userManager.Users.Any())
        {
            throw new Exception("Expected no users in the DB before initialization.");
        }

        string password = hostConfiguration["Custom:IdentityServer:ModeratorPassword"];

        string userName = "userA@example.com";
        ApplicationUser_DB userA = new ApplicationUser_DB
        {
            Email = userName,
            UserName = userName,
            EmailConfirmed = true,
        };
        usersOrchestrator.CreateUserAsync(
            userA,
            password
        ).GetAwaiter().GetResult();
        userManager.AddToRoleAsync(
            userA,
            Enum.GetName(IdentityRolesEnum.User)
        ).GetAwaiter().GetResult();

        string moderatorName = "moderatorA@example.com";
        ApplicationUser_DB moderatorA = new ApplicationUser_DB
        {
            Email = moderatorName,
            UserName = moderatorName,
            EmailConfirmed = true
        };
        usersOrchestrator.CreateUserAsync(
            moderatorA,
            password
        ).GetAwaiter().GetResult();
        userManager.AddToRoleAsync(
            moderatorA,
            Enum.GetName(IdentityRolesEnum.Moderator)
        ).GetAwaiter().GetResult();

        Console.WriteLine("Moderator email: " + moderatorA.Email);
        Console.WriteLine("Moderator password: " + password);

        return userA;
    }
}