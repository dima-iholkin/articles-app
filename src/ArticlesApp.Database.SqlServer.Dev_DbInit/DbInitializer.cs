using ArticlesApp.Core.Orchestrators;
using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer.Dev_DbInitializer.DataTables;
using ArticlesApp.Database.SqlServer.Dev_DbInitializer.LookupTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



namespace ArticlesApp.Database.SqlServer.Dev_DbInitializer;



public class DbInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _hostConfiguration;
    private readonly ILogger<DbInitializer> _logger;
    private readonly RoleManager<ApplicationUserRole_DB> _roleManager;
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly UsersOrchestrator _usersOrchestrator;

    public DbInitializer(
        RoleManager<ApplicationUserRole_DB> roleManager,
        UserManager<ApplicationUser_DB> userManager,
        ApplicationDbContext dbContext,
        UsersOrchestrator usersOrchestrator,
        ILogger<DbInitializer> logger,
        IConfiguration hostConfiguration
    )
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _dbContext = dbContext;
        _usersOrchestrator = usersOrchestrator;
        _logger = logger;
        _hostConfiguration = hostConfiguration;
    }



    public void InitializeDb()
    {
        if (_dbContext.Database.CanConnect())
        {
            throw new Exception(
                "The database is already present." +
                    "The initialization was canceled to ensure the database's integrity." +
                    "Delete the current database or change it's name."
            );
        }

        try
        {
            _dbContext.Database.Migrate();

            LookupTablesInitializer.InitializeDb(
                _roleManager,
                _dbContext
            );
            // Fill the lookup tables, like ArticleStates, SoftDeletionReasons, NotificationTypes.

            DataTablesInitializer.InitializeDb_Dev(
                _userManager,
                _dbContext,
                _usersOrchestrator,
                _hostConfiguration
            );
            // Fill the data tables, like Users, Articles, Notifications.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the DB.");

            throw;
        }
    }
}