using Domain.Enums.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Common
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        public required string Message { get; set; }

        public string? StackTrace { get; set; }

        public LogType LogType = LogType.None;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? UserId { get; set; }

        public Guid? PostId { get; set; }
    }
}
