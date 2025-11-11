using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.Notifications.ReadNotification;

public class ReadNotificationCommandHandler : IRequestHandler<ReadNotificationCommand, ApiResponse<Unit>>
{
    private readonly INotificationQueryRepository _notificationQueryRepository;
    private readonly INotificationCommandRepository _notificationCommandRepository;

    public ReadNotificationCommandHandler(INotificationQueryRepository notificationQueryRepository,
        INotificationCommandRepository notificationCommandRepository)
    {
        _notificationQueryRepository = notificationQueryRepository;
        _notificationCommandRepository = notificationCommandRepository;
    }
    
    public async Task<ApiResponse<Unit>> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationQueryRepository.GetAllNotificationsAsync()
            .FirstOrDefaultAsync(x => x.Id == request.NotificationId && x.UserId == request.AuthorizeData.UserId,
                cancellationToken);
        if (notification is null)
            return ApiResponse<Unit>.Failure("Notification not found");
        
        if (notification.IsRead)
            return ApiResponse<Unit>.Success(Unit.Value);
        
        await _notificationCommandRepository.SetReadNotificationAsync(notification.Id, true, cancellationToken);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}