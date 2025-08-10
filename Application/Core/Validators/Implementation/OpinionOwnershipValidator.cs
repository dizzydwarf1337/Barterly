using Application.Core.Validators.Interfaces;
using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;

namespace Application.Core.Validators.Implementation;

public class OpinionOwnershipValidator : IOpinionOwnershipValidator
{
    private readonly IPostOpinionQueryRepository _postOpinionQueryRepository;
    private readonly IUserOpinionQueryRepository _userOpinionQueryRepository;

    public OpinionOwnershipValidator(IUserOpinionQueryRepository userOpinionQueryRepository,
        IPostOpinionQueryRepository postOpinionQueryRepository)
    {
        _userOpinionQueryRepository = userOpinionQueryRepository;
        _postOpinionQueryRepository = postOpinionQueryRepository;
    }

    public async Task ValidateOpinionOwnership(Guid userId, Guid opinionId)
    {
        Opinion? opinion = null;
        try
        {
            opinion = await _userOpinionQueryRepository.GetUserOpinionByIdAsync(opinionId, CancellationToken.None);
        }
        catch
        {
            opinion = await _postOpinionQueryRepository.GetPostOpinionByIdAsync(opinionId, CancellationToken.None);
        }

        if (opinion.AuthorId != userId)
            throw new AccessForbiddenException("ValidateOpinionOwnerShip", userId.ToString(),
                "AuthorId and userId mismatch");
    }
}