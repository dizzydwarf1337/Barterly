using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Admins.Users.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ApiResponse<Unit>>
{
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IPostSettingsCommandRepository _postSettingsRepository;
    private readonly IPostQueryRepository _postQueryRepository;

    public DeleteUserCommandHandler(IUserCommandRepository userCommandRepository,
        IPostSettingsCommandRepository postSettingsRepository,
        IPostQueryRepository postQueryRepository)
    {
        _userCommandRepository = userCommandRepository;
        _postSettingsRepository = postSettingsRepository;
        _postQueryRepository = postQueryRepository;
    }
    
    public async Task<ApiResponse<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userCommandRepository.DeleteUser(request.Id, cancellationToken);
        var posts = await _postQueryRepository.GetAllPosts().Where(x => x.OwnerId == request.Id)
            .ToListAsync(cancellationToken);
        foreach (var post in posts)
        {
            await _postSettingsRepository.UpdatePostSettings(post.Id, cancellationToken, isDeleted: true);
        }

        return ApiResponse<Unit>.Success(Unit.Value);
    }
}