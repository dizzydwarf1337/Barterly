using Application.Core.ApiResponse;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Users.Opinions.DeleteOpinion;

public class DeleteOpinionCommandHandler : IRequestHandler<DeleteOpinionCommand, ApiResponse<Unit>>
{
    private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
    private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;

    public DeleteOpinionCommandHandler(IUserOpinionCommandRepository userOpinionCommandRepository,
        IPostOpinionCommandRepository postOpinionCommandRepository)
    {
        _userOpinionCommandRepository = userOpinionCommandRepository;
        _postOpinionCommandRepository = postOpinionCommandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(DeleteOpinionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _postOpinionCommandRepository.DeletePostOpinionAsync(request.OpinionId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            await _userOpinionCommandRepository.DeleteUserOpinionAsync(request.OpinionId, cancellationToken);
        }

        return ApiResponse<Unit>.Success(Unit.Value);
    }
}