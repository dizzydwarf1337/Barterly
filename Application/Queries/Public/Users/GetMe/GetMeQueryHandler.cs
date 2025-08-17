using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Queries.Public.Users.GetMe;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ApiResponse<GetMeQuery.Result>>
{
    private readonly IUserQueryRepository _userRepository;
    private readonly IUserFavPostQueryRepository _favPostRepository;
    private readonly INotificationQueryRepository _notificationRepository;

    public GetMeQueryHandler(IUserFavPostQueryRepository favPostRepository, IUserQueryRepository userRepository,
        INotificationQueryRepository notificationRepository)
    {
        _favPostRepository = favPostRepository;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
    }
    
    public async Task<ApiResponse<GetMeQuery.Result>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.AuthorizeData.UserId, cancellationToken);
        var favPostIds = (await _favPostRepository.GetUserFavPostsByUserIdAsync(user.Id, cancellationToken)).Select(x=>x.PostId).ToList();
        var notificationsCount = (await _notificationRepository.GetNotificationsByUserIdAsync(user.Id, cancellationToken)).Count;
        return ApiResponse<GetMeQuery.Result>.Success(new GetMeQuery.Result
        {
            Email = user.Email,
            FirstName = user.FirstName,
            Id = user.Id.ToString(),
            LastName = user.LastName,
            ProfilePicturePath = user.ProfilePicturePath,
            FavPostIds = favPostIds,
            NotificationCount = notificationsCount 
        });
    }
}