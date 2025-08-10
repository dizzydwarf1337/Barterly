using Application.Core.ApiResponse;
using Domain.Entities.Common;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Users.Opinions.EditOpinion;

public class EditOpinionCommandHandler : IRequestHandler<EditOpinionCommand, ApiResponse<EditOpinionCommand.Result>>
{
    private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
    private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;

    public EditOpinionCommandHandler(IPostOpinionCommandRepository postOpinionCommandRepository,
        IUserOpinionCommandRepository userOpinionCommandRepository)
    {
        _postOpinionCommandRepository = postOpinionCommandRepository;
        _userOpinionCommandRepository = userOpinionCommandRepository;
    }

    public async Task<ApiResponse<EditOpinionCommand.Result>> Handle(EditOpinionCommand request,
        CancellationToken cancellationToken)
    {
        Opinion opinion;
        Guid subjectId;
        try
        {
            var PostOpinion = await _postOpinionCommandRepository.UpdatePostOpinionAsync(request.OpinionId,
                request.Message, request.Rate, cancellationToken);
            opinion = PostOpinion;
            subjectId = PostOpinion.PostId;
        }
        catch (Exception)
        {
            var UserOpinion = await _userOpinionCommandRepository.UpdateUserOpinionAsync(request.OpinionId,
                request.Message, request.Rate, cancellationToken);
            opinion = UserOpinion;
            subjectId = UserOpinion.UserId;
        }

        return ApiResponse<EditOpinionCommand.Result>.Success(
            new EditOpinionCommand.Result(
                opinion.Id,
                opinion.AuthorId,
                subjectId,
                opinion.Content,
                opinion.Rate.Value,
                opinion.IsHidden,
                opinion.CreatedAt,
                opinion.LastUpdatedAt)
        );
    }
}