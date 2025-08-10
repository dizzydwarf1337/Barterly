using Application.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.Events.Users.UserCreated;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Enums.Users;
using MediatR;

namespace Application.Commands.Public.Accounts.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly IUserFactory _userFactory;

    public CreateUserCommandHandler(IMediator mediator, ILogService logService, IUserFactory userFactory)
    {
        _mediator = mediator;
        _logService = logService;
        _userFactory = userFactory;
    }

    public async Task<ApiResponse<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userFactory.CreateUser(request.FirstName, request.LastName, request.Email,
            request.Password, UserRoles.User);
        await _mediator.Publish(new UserCreatedEvent { Email = request.Email }, cancellationToken);
        await _logService.CreateLogAsync($"User created {user.Email}", cancellationToken, LogType.Information,
            userId: user.Id);
        return ApiResponse<Unit>.Success(Unit.Value, 201);
    }
}