using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Models.SoftDeletion;
using Serilog;



namespace ArticlesApp.Core.SoftDeletionReaper;



public class SoftDeletionReaper
{
    private readonly IUserRepository _userRepository;
    private readonly IArticlesRepository _articlesRepository;
    private readonly ILogger _logger;

    public SoftDeletionReaper(
        IUserRepository userRepository,
        IArticlesRepository articlesRepository,
        ILogger logger
    )
    {
        _userRepository = userRepository;
        _articlesRepository = articlesRepository;
        _logger = logger.ForContext<SoftDeletionReaper>();
    }



    public async Task RunReaperAsync(
        DateTime nowUtc,
        int softDeletionPeriod_Days
    )
    {
        _logger.Information("Starting the reaper.");

        DateTime olderThan_DateUtc = DateTime.UtcNow.AddDays(-softDeletionPeriod_Days);

        HardDeletedUsersCount deletedCount = await _userRepository.HardDeleteUsersAsync(olderThan_DateUtc);
        _logger.Information(
            "Hard-deleted {userCount} users and their {articleCount} articles.",
            deletedCount.Users,
            deletedCount.Articles
        );

        int deletedArticleCount = await _articlesRepository.HardDeleteArticlesAsync(olderThan_DateUtc);
        _logger.Information("Hard-deleted {articleCount} articles.", deletedArticleCount);
    }
}