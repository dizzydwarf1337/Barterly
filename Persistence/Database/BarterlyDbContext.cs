using Domain.Entities.Categories;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<UserFavouritePost>().HasKey(ufp => new { ufp.UserId, ufp.PostId });
            modelBuilder.Entity<VisitedPost>().HasKey(vp => new { vp.PostId, vp.UserId });
            modelBuilder.Entity<User>()
                .HasOne(u => u.Setting)
                .WithOne(us => us.User)
                .HasForeignKey<UserSettings>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserSettings>()
                .HasOne<User>(u => u.User)
                .WithOne(u => u.Setting)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(x => x.UserOpinions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserOpinion>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserOpinions)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Post>()
                .HasOne(x=>x.PostSettings)
                .WithOne(x=>x.Post)
                .HasForeignKey<PostSettings>(x=>x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PostSettings>()
                .HasOne(x => x.Post)
                .WithOne(x => x.PostSettings)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Post>().
                HasOne(x=>x.Promotion)
                .WithOne(x=>x.Post)
                .HasForeignKey<Promotion>(x=>x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Promotion>()
                .HasOne(x => x.Post)
                .WithOne(x => x.Promotion)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FavouriteCategory> FavouriteCategories { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MainAnnounsment> MainAnnounsments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts {  get; set; }
        public DbSet<CommonPost> CommonPosts { get; set; }
        public DbSet<WorkPost> WorkPosts { get; set; }
        public DbSet<RentPost> RentPosts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostSettings> PostSettings { get; set; }
        public DbSet<PostOpinion> PostOpinions { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ReportPost> ReportPosts { get; set; }
        public DbSet<ReportUser> ReportUsers { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<UserActivitySummary> UserActivities { get; set; }
        public DbSet<UserFavouritePost> UserFavouritePosts { get; set; }
        public DbSet<UserOpinion> UserOpinions { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<VisitedPost> VisitedPosts { get; set; }
        public DbSet<GlobalNotification> GlobalNotifications { get; set; }
    }
}
