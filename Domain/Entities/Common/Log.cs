using System.ComponentModel.DataAnnotations;
using Domain.Enums.Common;

namespace Domain.Entities.Common;

public class Log
{
    public LogType LogType = LogType.None;
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(500)] public required string Message { get; set; }

    public string? StackTrace { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? UserId { get; set; }

    public Guid? PostId { get; set; }
}