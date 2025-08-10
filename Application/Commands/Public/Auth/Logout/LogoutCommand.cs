using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Public.Auth.Logout;

public class LogoutCommand : AuthorizedRequest<Unit>
{
}