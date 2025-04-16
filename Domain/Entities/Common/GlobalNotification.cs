using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Common
{
    public class GlobalNotification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
