using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Public.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdQuery.Result>>
{
    private readonly IUserQueryRepository _userQueryRepository;
    public GetUserByIdQueryHandler(IUserQueryRepository userQueryRepository) 
        => _userQueryRepository = userQueryRepository;
    
    public async Task<ApiResponse<GetUserByIdQuery.Result>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUsers()
            .Include(x => x.Setting)
            .Where(x => x.Id == request.Id)
            .Where(x => !x.Setting.IsHidden && !x.Setting.IsBanned && !x.Setting.IsDeleted)
            .Select(x => new GetUserByIdQuery.Result(
                x.Id, x.FirstName, x.CreatedAt, x.LastName, x.ProfilePicturePath
            ))
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return ApiResponse<GetUserByIdQuery.Result>.Failure("Nie znaleziono użytkownika");
        return ApiResponse<GetUserByIdQuery.Result>.Success(user);
    }
}