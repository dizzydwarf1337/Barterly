using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Public.Users.GetUserData;

public class GetUserDataQueryHandler : IRequestHandler<GetUserDataQuery, ApiResponse<GetUserDataQuery.Result>>
{
    private readonly IUserQueryRepository _userQueryRepository;
    
    public GetUserDataQueryHandler(IUserQueryRepository userQueryRepository)
        => _userQueryRepository = userQueryRepository;
    
    public async Task<ApiResponse<GetUserDataQuery.Result>> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUsers()
            .Where(x => x.Id == request.UserId && 
                        (
                            !(x.Setting.IsBanned || x.Setting.IsDeleted || x.Setting.IsHidden) || 
                            x.Id == request.AuthorizeData!.UserId
                        )
            ).FirstOrDefaultAsync(cancellationToken);
        if (user == null) return ApiResponse<GetUserDataQuery.Result>.Failure("User not found", 404);
        return ApiResponse<GetUserDataQuery.Result>.Success(
            new GetUserDataQuery.Result(user.Id, user.FirstName, user.LastName, user.Bio, user.Country, user.City,
                user.Street, user.HouseNumber, user.PostalCode, user.ProfilePicturePath));
    }
}