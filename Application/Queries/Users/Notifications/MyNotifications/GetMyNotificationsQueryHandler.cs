using Application.Core.ApiResponse;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Queries.Users.Notifications.MyNotifications;

public class GetMyNotificationsQueryHandler : IRequestHandler<GetMyNotificationsQuery, ApiResponse<GetMyNotificationsQuery.Result>>
{
    private readonly INotificationQueryRepository  _notificationQueryRepository;
    
    public GetMyNotificationsQueryHandler(INotificationQueryRepository notificationQueryRepository)
        => _notificationQueryRepository = notificationQueryRepository;
    
    public async Task<ApiResponse<GetMyNotificationsQuery.Result>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications =
            await _notificationQueryRepository.GetNotificationsByUserIdAsync(request.AuthorizeData.UserId, cancellationToken);
        return ApiResponse<GetMyNotificationsQuery.Result>.Success(new GetMyNotificationsQuery.Result(notifications));
    }
}