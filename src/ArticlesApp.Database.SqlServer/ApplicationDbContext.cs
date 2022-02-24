using ArticlesApp.Core.Entities.Article;
using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Entities.SoftDeletion;
using ArticlesApp.Database.Models;
using ArticlesApp.Database.SqlServer.Models;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;



namespace ArticlesApp.Database.SqlServer;



public class ApplicationDbContext :
    IdentityDbContext<ApplicationUser_DB, ApplicationUserRole_DB, string>,
    IPersistedGrantDbContext
{
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions
    ) : base(options)
    {
        _operationalStoreOptions = operationalStoreOptions;

        Articles = Set<Article_SqlServer>();
        ArticleStates = Set<ArticleState>();

        Notifications = Set<Notification_SqlServer>();
        NotificationTypes = Set<NotificationType>();

        SoftDeletionReasons = Set<SoftDeletionReason>();

        AccountMerges = Set<AccountMerge_SqlServer>();

        PersistedGrants = Set<PersistedGrant>();
        DeviceFlowCodes = Set<DeviceFlowCodes>();
    }



    // Article entities:

    public DbSet<Article_SqlServer> Articles { get; set; }
    public DbSet<ArticleState> ArticleStates { get; set; }

    // Notification entities:

    public DbSet<Notification_SqlServer> Notifications { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }

    // Soft-deletion entities:

    public DbSet<SoftDeletionReason> SoftDeletionReasons { get; set; }

    // Identity entities:

    public DbSet<AccountMerge_SqlServer> AccountMerges { get; set; }

    public DbSet<PersistedGrant> PersistedGrants { get; set; }
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

    Task<int> IPersistedGrantDbContext.SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);

        // Articles:

        builder.Entity<Article_SqlServer>()
            .HasOne<ApplicationUser_DB>()
            .WithMany()
            .HasForeignKey(ar => ar.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Article_SqlServer>()
            .HasOne<ArticleState>(ar => ar.ArticleState)
            .WithMany()
            .HasForeignKey(ar => ar.ArticleStateId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Article_SqlServer>()
            .HasOne<ApplicationUser_DB>()
            .WithMany()
            .HasForeignKey(ar => ar.ArticleStateId_LastChangedBy_ModeratorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Article_SqlServer>()
            .HasIndex(ar => ar.ArticleStateId_LastChangedBy_ModeratorId)
            .HasFilter("[" + nameof(Article_SqlServer.ArticleStateId_LastChangedBy_ModeratorId) + "] IS NOT NULL");

        builder.Entity<Article_SqlServer>()
            .Property(ar => ar.SubmittedAt_DateUtc)
            .HasColumnType<DateTime>("datetime2(2)");

        builder.Entity<Article_SqlServer>()
            .Property(ar => ar.ArticleStateId_LastChangedAt_DateUtc)
            .HasColumnType<DateTime?>("datetime2(2)");

        builder.Entity<Article_SqlServer>()
            .Property(ar => ar.SoftDeletedAt_DateUtc)
            .HasColumnType<DateTime?>("datetime2(2)");

        builder.Entity<Article_SqlServer>()
            .HasIndex(ar => ar.SoftDeletedAt_DateUtc)
            .HasFilter("[" + nameof(Article_SqlServer.SoftDeletedAt_DateUtc) + "] IS NOT NULL");

        builder.Entity<Article_SqlServer>()
            .HasOne<SoftDeletionReason>(ar => ar.SoftDeletionReason)
            .WithMany()
            .HasForeignKey(ar => ar.SoftDeletionReason_ReasonId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Article_SqlServer>()
            .Property(ar => ar.VersionId)
            .HasColumnType("smallint")
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        // Set up the concurrency column. It does everything,
        // except for autoincrementing the value and
        // checking that the prev value is not overriden by an incorrect value.
        // This has to be done in the code, for now.

        // ArticleStates:

        builder.Entity<ArticleState>()
            .Property(st => st.Id)
            .HasColumnType("tinyint");

        // Notifications:

        builder.Entity<Notification_SqlServer>()
            .Property(n => n.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Notification_SqlServer>()
            .HasKey(n => new { n.Reciever_UserId, n.Id })
            .IsClustered(true);

        builder.Entity<Notification_SqlServer>()
            .HasIndex(notif => notif.Id)
            .IsUnique(true);

        builder.Entity<Notification_SqlServer>()
            .HasOne(notif => notif.Reciever)
            .WithMany()
            .HasForeignKey(notif => notif.Reciever_UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Notification_SqlServer>()
            .HasOne(notif => notif.ReferencedArticle)
            .WithMany()
            .HasForeignKey(notif => notif.ReferencedArticle_ArticleId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Notification_SqlServer>()
            .HasOne<NotificationType>(notif => notif.NofiticationType)
            .WithMany()
            .HasForeignKey(notif => notif.NotificationType_TypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Notification_SqlServer>()
            .Property(nt => nt.CreatedAt_DateUtc)
            .HasColumnType<DateTime>("datetime2(2)");

        builder.Entity<Notification_SqlServer>()
            .Property(nt => nt.ReadAt_DateUtc)
            .HasColumnType<DateTime?>("datetime2(2)");

        // NotificationTypes:

        builder.Entity<NotificationType>()
            .Property(nt => nt.Id)
            .HasColumnType("tinyint");

        // Users:

        builder.Entity<ApplicationUser_DB>()
            .Property(user => user.Id)
            .HasColumnType<string>("char(36)");

        builder.Entity<ApplicationUser_DB>()
            .HasIndex(user => user.SoftDeletedAt_DateUtc)
            .HasFilter($"[{nameof(ApplicationUser_DB.SoftDeletedAt_DateUtc)}] IS NOT NULL");

        builder.Entity<ApplicationUser_DB>()
            .Property(user => user.SoftDeletedAt_DateUtc)
            .HasColumnType<DateTime?>("datetime2(2)");

        builder.Entity<ApplicationUser_DB>()
            .Property(user => user.LockoutEnd)
            .HasColumnType<DateTimeOffset?>("datetimeoffset(2)");

        builder.Entity<ApplicationUser_DB>()
            .Property(user => user.AccessFailedCount)
            .HasColumnType<int>("tinyint");

        builder.Entity<ApplicationUser_DB>()
            .HasIndex(user => user.NormalizedEmail)
            .HasFilter($"[{nameof(ApplicationUser_DB.NormalizedEmail)}] IS NOT NULL")
            .IsUnique();
        // Enforce a unique email for the users who have an email. This means the local logins.

        // Roles:

        builder.Entity<ApplicationUserRole_DB>()
            .Property(role => role.Id)
            .HasColumnType<string>("char(36)");

        // AccountMerges:

        //builder.Entity<AccountsMerge>()
        //    .Property(acc => acc.Id)
        //    .ValueGeneratedOnAdd();

        builder.Entity<AccountMerge_SqlServer>()
            .HasOne<ApplicationUser_DB>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(acc => acc.PrimaryUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<AccountMerge_SqlServer>()
            .Property(acc => acc.PrimaryUserId)
            .HasColumnType<string>("char(36)");

        builder.Entity<AccountMerge_SqlServer>()
            .HasOne<ApplicationUser_DB>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(acc => acc.SecondaryUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<AccountMerge_SqlServer>()
            .Property(acc => acc.SecondaryUserId)
            .HasColumnType<string>("char(36)");

        // SoftDeletionReasons:

        builder.Entity<SoftDeletionReason>()
            .Property(re => re.Id)
            .HasColumnType("tinyint");
    }



    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder
    //      .LogTo(Console.WriteLine)
    //      .EnableSensitiveDataLogging();

    //    base.OnConfiguring(optionsBuilder);
    //}
}