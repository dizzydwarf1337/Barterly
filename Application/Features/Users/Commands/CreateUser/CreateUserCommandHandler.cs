using API.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.Features.Users.Events.UserCreated;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Unit>>
    {
        private readonly IMediator _mediator;
        private readonly ILogService _logService;
        private readonly IUserFactory _userFactory;

        public CreateUserCommandHandler(IMediator mediator, ILogService logService, IUserFactory userFactory)
        {
            _mediator = mediator;
            _logService = logService;
            _userFactory = userFactory;
        }

        public async Task<ApiResponse<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userFactory.CreateUser(request.registerDto);

            await _mediator.Publish(new UserCreatedEvent { Email = request.registerDto.Email }, cancellationToken);
            await _logService.CreateLogAsync($"User created {user.Email}", LogType.Information, userId: user.Id);
            return ApiResponse<Unit>.Success(Unit.Value, 201);
        }
    }
}
