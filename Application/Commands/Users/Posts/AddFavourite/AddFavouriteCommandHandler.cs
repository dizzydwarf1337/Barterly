using Application.Core.ApiResponse;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Commands.Users.Posts.AddFavourite;

public class AddFavouriteCommandHandler : IRequestHandler<AddFavouriteCommand, ApiResponse<Unit>>
{
    private readonly IUserFavPostCommandRepository _commandRepository;
    private readonly IUserFavPostQueryRepository _queryRepository;

    public AddFavouriteCommandHandler(IUserFavPostCommandRepository commandRepository,
        IUserFavPostQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(AddFavouriteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var fav = await _queryRepository.GetUserFavPostByIdAsync(request.Id, request.AuthorizeData.UserId, cancellationToken);
            await _commandRepository.DeleteUserFavPostAsync(fav.PostId, fav.UserId, cancellationToken);
            return ApiResponse<Unit>.Success(Unit.Value);
        }
        catch
        {
            var favPost = new UserFavouritePost
                { UserId = request.AuthorizeData.UserId, PostId = request.Id, CreatedAt = DateTime.UtcNow };
            await _commandRepository.CreateUserFavPostAsync(favPost, cancellationToken);
            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}