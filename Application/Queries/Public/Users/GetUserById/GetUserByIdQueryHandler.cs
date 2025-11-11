using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Public.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdQuery.Result>>
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserQueryRepository userQueryRepository, IMapper mapper)
    {
        _userQueryRepository = userQueryRepository;
        _mapper = mapper;
    } 
        
    
    public async Task<ApiResponse<GetUserByIdQuery.Result>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUsers()
            .Include(x => x.Setting)
            .Include(x => x.UserPosts)
            .Where(x => x.Id == request.Id)
            .Where(x => !x.Setting.IsHidden && !x.Setting.IsBanned && !x.Setting.IsDeleted)
            .Select(x => new GetUserByIdQuery.Result(
                x.Id, x.FirstName, 
                x.CreatedAt,
                x.LastName,
                x.ProfilePicturePath, 
                _mapper.Map<ICollection<PostPreviewDto>>(x.UserPosts.Where(y=> !y.PostSettings.IsDeleted && !y.PostSettings.IsHidden))
            ))
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return ApiResponse<GetUserByIdQuery.Result>.Failure("Nie znaleziono użytkownika");
        return ApiResponse<GetUserByIdQuery.Result>.Success(user);
    }
}