using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Admins.Users.UpdateUserSettings;

public class UpdateUserSettingsCommandHandler : IRequestHandler<UpdateUserSettingsCommand, ApiResponse<Unit>>
{
    private readonly IUserSettingCommandRepository _userSettingCommandRepository;
    
    public UpdateUserSettingsCommandHandler(IUserSettingCommandRepository userSettingCommandRepository)
        => _userSettingCommandRepository = userSettingCommandRepository;
    
    public async Task<ApiResponse<Unit>> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Id);
        await _userSettingCommandRepository.UpdateUserSettingsByUserId(
            request.Id,
            request.IsHidden,
            request.IsDeleted,
            request.IsBanned,
            request.IsChatRestricted,
            request.IsOpinionRestricted,
            request.IsPostRestricted,
            cancellationToken);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}