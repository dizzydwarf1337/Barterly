using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public class User : IdentityUser<Guid>
{
    [Required] public required string FirstName { get; set; }

    [Required] public required string LastName { get; set; }

    public string? Bio { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? HouseNumber { get; set; }

    public string? PostalCode { get; set; }

    public string? ProfilePicturePath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime LastSeen { get; set; } = DateTime.Now;

    public Guid? UserActivitySummaryId { get; set; }
    public Guid? UserSettingId { get; set; }

    [ForeignKey("UserSettingId")] public virtual UserSettings? Setting { get; set; }

    public virtual ICollection<UserChat>? UserChats { get; set; }
    public virtual ICollection<UserFavouritePost>? FavouritePosts { get; set; }
    public virtual ICollection<FavouriteCategory>? FavouriteCategories { get; set; }
    public virtual ICollection<VisitedPost>? VisitedPosts { get; set; }
    public virtual UserActivitySummary? UserActivitySummary { get; set; }
    public virtual ICollection<Notification>? Notifications { get; set; }
    public virtual ICollection<Opinion>? CreatedOpinions { get; set; }
    public virtual ICollection<UserOpinion>? UserOpinions { get; set; }
    public virtual ICollection<Report>? Reports { get; set; }
    public virtual ICollection<Post>? UserPosts { get; set; }
    public virtual ICollection<SearchHistory>? SearchHistory { get; set; }
    public virtual ICollection<Transaction>? Transactions { get; set; }
    public virtual ICollection<Payment>? Payments { get; set; }
}