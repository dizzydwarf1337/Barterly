using System.ComponentModel.DataAnnotations;
namespace Domain.Entities.Chat;

public class Chat
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime  CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid User1 { get; set; }
    public Guid User2 { get; set; }
    public virtual List<Message> Messages { get; set; } = new List<Message>();
}