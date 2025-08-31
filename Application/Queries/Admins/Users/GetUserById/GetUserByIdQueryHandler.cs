using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Admins.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdQuery.Result>>
{
    private readonly IUserQueryRepository _userQueryRepository;
    
    public GetUserByIdQueryHandler(IUserQueryRepository userQueryRepository)
        => _userQueryRepository = userQueryRepository;
    
    public async Task<ApiResponse<GetUserByIdQuery.Result>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUsers().Include(x=>x.Setting)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if(user is null)
            return ApiResponse<GetUserByIdQuery.Result>.Failure("User not found");
        var settings = user.Setting;
        var result = new GetUserByIdQuery.Result
        (
            new GetUserByIdQuery.UserData(user.FirstName, user.LastName, user.Email, user.Bio, user.Country, user.City,
                user.Street, user.HouseNumber, user.PostalCode, user.ProfilePicturePath, user.CreatedAt, user.LastSeen),
            new GetUserByIdQuery.UserSettings(settings.Id, settings.IsHidden, settings.IsDeleted, settings.IsBanned,
                settings.IsPostRestricted, settings.IsPostRestricted, settings.IsChatRestricted)
        );
        
        return ApiResponse<GetUserByIdQuery.Result>.Success(result);
    }
}