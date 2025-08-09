using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Admins.Opinions.DeleteOpinion;

public class DeleteOpinionCommandHandler : IRequestHandler<DeleteOpinionCommand, ApiResponse<Unit>>
{
    private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
    private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;

    public DeleteOpinionCommandHandler(IPostOpinionCommandRepository postOpinionCommandRepository,
        IUserOpinionCommandRepository userOpinionCommandRepository)
    {
        _postOpinionCommandRepository = postOpinionCommandRepository;
        _userOpinionCommandRepository = userOpinionCommandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(DeleteOpinionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _postOpinionCommandRepository.DeletePostOpinionAsync(request.OpinionId, cancellationToken);
        }
        catch (Exception)
        {
            await _userOpinionCommandRepository.DeleteUserOpinionAsync(request.OpinionId, cancellationToken);
        }

        return ApiResponse<Unit>.Success(Unit.Value);
    }
}