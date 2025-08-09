using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Moderators.Opinions.SetOpinionHidden;

public class SetOpinionHiddenCommandHandler : IRequestHandler<SetOpinionHiddenCommand, ApiResponse<Unit>>
{
    private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
    private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;

    public SetOpinionHiddenCommandHandler(IPostOpinionCommandRepository postOpinionCommandRepository,
        IUserOpinionCommandRepository userOpinionCommandRepository)
    {
        _postOpinionCommandRepository = postOpinionCommandRepository;
        _userOpinionCommandRepository = userOpinionCommandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(SetOpinionHiddenCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _postOpinionCommandRepository.SetHiddenPostOpinionAsync(request.OpinionId, request.IsHidden,
                cancellationToken);
        }
        catch (Exception)
        {
            await _userOpinionCommandRepository.SetHiddenUserOpinionAsync(request.OpinionId, request.IsHidden,
                cancellationToken);
        }

        return ApiResponse<Unit>.Success(Unit.Value);
    }
}