using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Orders.GetMyPlacedOrders;

public class GetMyPlacedOrdersQueryHandler : IRequestHandler<GetMyPlacedOrdersQuery, ApiResponse<ICollection<GetMyPlacedOrdersQuery.Result>>>
{
    private readonly IOrderQueryRepository _orderQueryRepository;
    public GetMyPlacedOrdersQueryHandler(IOrderQueryRepository orderQueryRepository)
        => _orderQueryRepository = orderQueryRepository;
    
    public async Task<ApiResponse<ICollection<GetMyPlacedOrdersQuery.Result>>> Handle(GetMyPlacedOrdersQuery request, CancellationToken cancellationToken)
    {
        var results = await _orderQueryRepository.GetOrders()
            .Where(x => x.SellerId == request.AuthorizeData!.UserId)
            .Select(x => new GetMyPlacedOrdersQuery.Result(
                x.Id,
                new GetMyPlacedOrdersQuery.OrderUser(
                    x.Customer.Id,
                    x.Customer.FirstName,
                    x.Customer.LastName,
                    x.Customer.ProfilePicturePath
                ),
                new GetMyPlacedOrdersQuery.OrderUser(
                    x.Seller.Id,
                    x.Seller.FirstName,
                    x.Seller.LastName,
                    x.Seller.ProfilePicturePath
                ),
                new GetMyPlacedOrdersQuery.OrderPost(
                    x.Post.Id,
                    x.Price,
                    x.Post.Currency,
                    x.Post.Title,
                    x.Post.MainImageUrl
                ),
                x.Status,
                x.CreatedAt,
                x.UpdatedAt
            ))
            .ToListAsync(cancellationToken);
        return ApiResponse<ICollection<GetMyPlacedOrdersQuery.Result>>.Success(results);
    }
}