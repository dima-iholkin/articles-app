﻿// <auto-generated />
using System;
using ArticlesApp.Database.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ArticlesApp.Core.Entities.Article.ArticleState", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ArticleStates");
                });

            modelBuilder.Entity("ArticlesApp.Core.Entities.Notification.NotificationType", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationTypes");
                });

            modelBuilder.Entity("ArticlesApp.Core.Entities.SoftDeletion.SoftDeletionReason", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SoftDeletionReasons");
                });

            modelBuilder.Entity("ArticlesApp.Database.Models.ApplicationUser_DB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<byte>("AccessFailedCount")
                        .HasColumnType("tinyint");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SoftDeletedAt_DateUtc")
                        .HasColumnType("datetime2(2)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .IsUnique()
                        .HasDatabaseName("EmailIndex")
                        .HasFilter("[NormalizedEmail] IS NOT NULL");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("SoftDeletedAt_DateUtc")
                        .HasFilter("[SoftDeletedAt_DateUtc] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ArticlesApp.Database.Models.ApplicationUserRole_DB", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("ArticlesApp.Database.SqlServer.Models.AccountMerge_SqlServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt_DateUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PrimaryUserConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PrimaryUserId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<bool>("SecondaryUserConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecondaryUserId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryUserId");

                    b.HasIndex("SecondaryUserId");

                    b.ToTable("AccountMerges");
                });

            modelBuilder.Entity("ArticlesApp.Database.SqlServer.Models.Article_SqlServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte>("ArticleStateId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("ArticleStateId_LastChangedAt_DateUtc")
                        .HasColumnType("datetime2(2)");

                    b.Property<string>("ArticleStateId_LastChangedBy_ModeratorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("SoftDeletedAt_DateUtc")
                        .HasColumnType("datetime2(2)");

                    b.Property<byte?>("SoftDeletionReason_ReasonId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("SubmittedAt_DateUtc")
                        .HasColumnType("datetime2(2)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("VersionId")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("ArticleStateId");

                    b.HasIndex("ArticleStateId_LastChangedBy_ModeratorId")
                        .HasFilter("[ArticleStateId_LastChangedBy_ModeratorId] IS NOT NULL");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SoftDeletedAt_DateUtc")
                        .HasFilter("[SoftDeletedAt_DateUtc] IS NOT NULL");

                    b.HasIndex("SoftDeletionReason_ReasonId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("ArticlesApp.Database.SqlServer.Models.Notification_SqlServer", b =>
                {
                    b.Property<string>("Reciever_UserId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt_DateUtc")
                        .HasColumnType("datetime2(2)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte?>("NotificationType_TypeId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("ReadAt_DateUtc")
                        .HasColumnType("datetime2(2)");

                    b.Property<int?>("ReferencedArticle_ArticleId")
                        .HasColumnType("int");

                    b.HasKey("Reciever_UserId", "Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Reciever_UserId", "Id"));

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NotificationType_TypeId");

                    b.HasIndex("ReferencedArticle_ArticleId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes", (string)null);
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("ConsumedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.HasIndex("SubjectId", "SessionId", "Type");

                    b.ToTable("PersistedGrants", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ArticlesApp.Database.SqlServer.Models.AccountMerge_SqlServer", b =>
                {
                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("PrimaryUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("SecondaryUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("ArticlesApp.Database.SqlServer.Models.Article_SqlServer", b =>
                {
                    b.HasOne("ArticlesApp.Core.Entities.Article.ArticleState", "ArticleState")
                        .WithMany()
                        .HasForeignKey("ArticleStateId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("ArticleStateId_LastChangedBy_ModeratorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ArticlesApp.Core.Entities.SoftDeletion.SoftDeletionReason", "SoftDeletionReason")
                        .WithMany()
                        .HasForeignKey("SoftDeletionReason_ReasonId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ArticleState");

                    b.Navigation("SoftDeletionReason");
                });

            modelBuilder.Entity("ArticlesApp.Database.SqlServer.Models.Notification_SqlServer", b =>
                {
                    b.HasOne("ArticlesApp.Core.Entities.Notification.NotificationType", "NofiticationType")
                        .WithMany()
                        .HasForeignKey("NotificationType_TypeId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", "Reciever")
                        .WithMany()
                        .HasForeignKey("Reciever_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArticlesApp.Database.SqlServer.Models.Article_SqlServer", "ReferencedArticle")
                        .WithMany()
                        .HasForeignKey("ReferencedArticle_ArticleId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("NofiticationType");

                    b.Navigation("Reciever");

                    b.Navigation("ReferencedArticle");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("ArticlesApp.Database.Models.ApplicationUserRole_DB", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("ArticlesApp.Database.Models.ApplicationUserRole_DB", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ArticlesApp.Database.Models.ApplicationUser_DB", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
