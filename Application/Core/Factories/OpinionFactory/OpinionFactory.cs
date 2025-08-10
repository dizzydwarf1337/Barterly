using Application.Core.Factories.Interfaces;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;

namespace Application.Core.Factories.OpinionFactory;

public class OpinionFactory : IOpinionFactory
{
    private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
    private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;

    public OpinionFactory(IUserOpinionCommandRepository userOpinionCommandRepository,
        IPostOpinionCommandRepository postOpinionCommandRepository)
    {
        _userOpinionCommandRepository = userOpinionCommandRepository;
        _postOpinionCommandRepository = postOpinionCommandRepository;
    }

    public async Task<Opinion> CreateOpinionAsync(Guid AuthorId, Guid SubjectId, string Content, int Rate,
        string OpinionType, CancellationToken token)
    {
        dynamic opinionDb;
        if (OpinionType == "Post")
        {
            var opinion = new PostOpinion
            {
                AuthorId = AuthorId,
                Content = Content,
                PostId = SubjectId,
                Rate = Rate
            };
            opinionDb = await _postOpinionCommandRepository.CreatePostOpinionAsync(opinion, token);
        }
        else if (OpinionType == "User")
        {
            var opinion = new UserOpinion
            {
                AuthorId = AuthorId,
                Content = Content,
                UserId = SubjectId,
                Rate = Rate
            };
            opinionDb = await _userOpinionCommandRepository.CreateUserOpinionAsync(opinion, token);
        }
        else
        {
            throw new EntityCreatingException("Opinion", "OpinionFactory");
        }

        return opinionDb;
    }
}