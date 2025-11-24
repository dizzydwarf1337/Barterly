using System.Text.Json.Serialization;

namespace Domain.Entities.Orders.Types;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Paid,
    Shipped,
    Delivered,
    Canceled
}