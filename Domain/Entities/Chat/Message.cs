using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Users;

namespace Domain.Entities.Chat;

public class Message
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid ChatId { get; set; }
    
    public Guid? PostId { get; set; }
    
    public string Content { get; set; }

    public bool IsRead { get; set; } = false;
    
    public bool? IsPaid  { get; set; } = false;
    
    public MessageType Type { get; set; }
    
    public Guid SenderId { get; set; }
    
    public Guid ReceiverId { get; set; }
    
    public Guid? ReadBy { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ReadAt { get; set; }
    
    public DateTime? AcceptedAt { get; set; }
    
    [Column(TypeName = "decimal(8,2)")]
    public decimal? Price { get; set; }

    public bool? IsAccepted { get; set; }
    
    [ForeignKey("SenderId")]
    public virtual User Sender { get; set; }
    [ForeignKey("ReceiverId")]
    public virtual User Receiver { get; set; }
    [ForeignKey("ChatId")]
    public virtual Chat Chat { get; set; }
}

public enum MessageType {
    Common,
    Proposal
}