using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string? Bio { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Street { get; set; }
        
        [Required]
        public string HouseNumber { get; set; }
        
        [Required]
        public string PostalCode { get; set; }
        
        public string? ProfilePicturePath { get; set; }
        
        public DateTime CreatedAt { get; set; } = new DateTime();
        
        public DateTime LastSeen { get; set; } = new DateTime();
        
        public Guid UserSettingId { get; set; }
        
        [ForeignKey("UserSettingId")]
        public virtual UserSetting Setting { get; set; }

        public virtual ICollection<UserChat>? UserChats { get; set; }
        public virtual ICollection<UserFavouritePost>? FavouritePosts { get; set; }
        public virtual ICollection<FavouriteCategory>? FavouriteCategories { get; set; }
        public virtual ICollection<VisitedPost>? VisitedPosts { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Opinion>? CreatedOpinions { get; set; }
        public virtual ICollection<UserOpinion>? UserOpinions { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
        public virtual ICollection<Post>? UserPosts { get; set; }
        public virtual ICollection<SearchHistory>? SearchHistory { get; set; } 
        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
