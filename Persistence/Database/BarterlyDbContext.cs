using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Database
{
    public class BarterlyDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public BarterlyDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavouriteCategory>().HasKey(fc => new { fc.UserId, fc.CategoryId });
            modelBuilder.Entity<SessionFavouritePost>().HasKey(sfp => new { sfp.SessionId, sfp.PostId });
            modelBuilder.Entity<UserFavouritePost>().HasKey(ufp => new { ufp.UserId, ufp.PostId });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DeletedPost> DeletedPosts { get; set; }
        public DbSet<DeletedUser> DeletedUsers { get; set; }
        public DbSet<FavouriteCategory> FavouriteCategories { get; set; }
        public DbSet<SessionFavouritePost> SessionFavouritePosts { get; set; }
        public DbSet<UserFavouritePost> UserFavorites { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MainAnnounsment> MainAnnounsments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostOpinion> PostOpinions { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ReportPost> ReportPosts { get; set; }
        public DbSet<ReportUser> ReportUsers { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; } 
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<UserActivitySummary> UserActivities { get; set; }
        public DbSet<UserFavouritePost> UserFavouritePosts { get; set; }
        public DbSet<UserOpinion> UserOpinions { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<VisitedPost> VisitedPosts { get; set; }
        public DbSet<GlobalNotification> GlobalNotifications { get; set; }
    }
}
