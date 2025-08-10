using Application.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.Events.Users.UserCreated;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using MediatR;
using Persistence.Database;

namespace Application.Commands.Admins.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly IUserFactory _userFactory;

    public CreateUserCommandHandler(IMediator mediator, ILogService logService, IUserFactory userFactory,
        BarterlyDbContext context)
    {
        _mediator = mediator;
        _logService = logService;
        _userFactory = userFactory;
    }

    public async Task<ApiResponse<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await CreateUser(request, cancellationToken);
        await _mediator.Publish(new UserCreatedEvent { Email = request.Email }, cancellationToken);
        return ApiResponse<Unit>.Success(Unit.Value, 201);
    }

    public async Task<User> CreateUser(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userFactory.CreateUser(request.FirstName, request.LastName, request.Email,
            request.Password, request.Role);
        await _logService.CreateLogAsync($"User created {user.Email}", cancellationToken, LogType.Information,
            userId: user.Id);
        return user;
    }
}