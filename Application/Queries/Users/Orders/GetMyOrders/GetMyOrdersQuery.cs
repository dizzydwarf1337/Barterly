using Application.Core.MediatR.Requests;
using Domain.Entities.Orders.Types;
using Domain.Enums.Posts;

namespace Application.Queries.Users.Orders.GetMyOrders;

public class GetMyOrdersQuery : UserRequest<ICollection<GetMyOrdersQuery.Result>>
{
    public record Result(Guid Id, OrderUser Buyer, OrderUser Seller, OrderPost Post, OrderStatus Status, DateTime CreatedAt, DateTime? UpdatedAt);
    public record OrderUser(Guid Id, string FirstName, string LastName, string? ImagePath);
    public record OrderPost(Guid Id, decimal Price, PostCurrency Currency, string Name, string? ImagePath);
}