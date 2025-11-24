using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Orders.GetMyOrders;

public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, ApiResponse<ICollection<GetMyOrdersQuery.Result>>>
{
    private readonly IOrderQueryRepository _orderQueryRepository;
    public GetMyOrdersQueryHandler(IOrderQueryRepository orderQueryRepository)
        =>  _orderQueryRepository = orderQueryRepository;

    public async Task<ApiResponse<ICollection<GetMyOrdersQuery.Result>>> Handle(GetMyOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var results = await _orderQueryRepository.GetOrders()
            .Where(x => x.CustomerId == request.AuthorizeData!.UserId)
            .Select(x => new GetMyOrdersQuery.Result(
                x.Id,
                new GetMyOrdersQuery.OrderUser(
                    x.Customer.Id,
                    x.Customer.FirstName,
                    x.Customer.LastName,
                    x.Customer.ProfilePicturePath
                ),
                new GetMyOrdersQuery.OrderUser(
                    x.Seller.Id,
                    x.Seller.FirstName,
                    x.Seller.LastName,
                    x.Seller.ProfilePicturePath
                ),
                new GetMyOrdersQuery.OrderPost(
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
        return ApiResponse<ICollection<GetMyOrdersQuery.Result>>.Success(results);
    }
}