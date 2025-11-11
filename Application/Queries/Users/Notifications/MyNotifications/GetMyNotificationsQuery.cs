using Application.Core.MediatR.Requests;
using Domain.Entities.Users;

namespace Application.Queries.Users.Notifications.MyNotifications;

public class GetMyNotificationsQuery : UserRequest<GetMyNotificationsQuery.Result>
{
    public record Result(IEnumerable<Notification> Notifications);
}