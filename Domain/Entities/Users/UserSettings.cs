using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users
{
    public class UserSettings
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public bool IsHidden { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public bool IsBanned { get; set; } = false;

        public bool IsPostRestricted { get; set; } = false;

        public bool IsOpinionRestricted { get; set; } = false;

        public bool IsChatRestricted { get; set; } = false;
        public virtual User User { get; set; }


    }
}
