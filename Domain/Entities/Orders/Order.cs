using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Orders.Types;
using Domain.Entities.Posts;
using Domain.Entities.Users;

namespace Domain.Entities.Orders;

public class Order
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid PostId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } 
    [Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }
    public OrderStatus Status { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public virtual User Customer { get; set; }
    
    [ForeignKey(nameof(SellerId))]
    public virtual User Seller { get; set; }
    
    [ForeignKey(nameof(PostId))]
    public virtual Post Post { get; set; }
}