using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Admins.Posts.UpdatePostSettings;

public class UpdatePostSettingsCommandHandler : IRequestHandler<UpdatePostSettingsCommand, ApiResponse<Unit>>
{
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
    
    public UpdatePostSettingsCommandHandler(IPostSettingsCommandRepository postSettingsCommandRepository)
        => _postSettingsCommandRepository = postSettingsCommandRepository;
    
    public async Task<ApiResponse<Unit>> Handle(UpdatePostSettingsCommand request, CancellationToken cancellationToken)
    {
        await _postSettingsCommandRepository.UpdatePostSettings(request.Id, cancellationToken, request.IsHidden, request.IsDeleted, request.PostStatusType, request.RejectionMessage);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}