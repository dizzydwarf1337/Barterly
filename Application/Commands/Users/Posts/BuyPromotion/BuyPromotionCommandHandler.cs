using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.Posts.BuyPromotion;

public class BuyPromotionCommandHandler : IRequestHandler<BuyPromotionCommand, ApiResponse<Unit>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IPromotionQueryRepository _promotionQueryRepository;
    private readonly IPromotionCommandRepository _promotionCommandRepository;

    public BuyPromotionCommandHandler(IPostQueryRepository postQueryRepository,
        IPromotionQueryRepository promotionQueryRepository, IPromotionCommandRepository promotionCommandRepository)
    {
        _postQueryRepository = postQueryRepository;
        _promotionQueryRepository = promotionQueryRepository;
        _promotionCommandRepository = promotionCommandRepository;
    }
    
    public async Task<ApiResponse<Unit>> Handle(BuyPromotionCommand request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetAllPosts()
            .Where(x => x.Id == request.PostId && x.OwnerId == request.AuthorizeData!.UserId).FirstOrDefaultAsync(cancellationToken);
        if(post == null)
            return ApiResponse<Unit>.Failure("Post not found", 404);
        var promotion = await _promotionQueryRepository.GetPromotionByPostIdAsync(post.Id, cancellationToken);
        promotion.Type = request.PostPromotionType;
        promotion.StartDate = DateTime.UtcNow;
        promotion.EndDate = promotion.StartDate.AddMonths(1);
        await _promotionCommandRepository.UpdatePromotionAsync(promotion, cancellationToken);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}