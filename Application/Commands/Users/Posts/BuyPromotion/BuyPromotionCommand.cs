using Application.Core.MediatR.Requests;
using Domain.Enums.Posts;
using MediatR;

namespace Application.Commands.Users.Posts.BuyPromotion;

public class BuyPromotionCommand : UserRequest<Unit>
{
    public Guid PostId { get; set; }
    public PostPromotionType  PostPromotionType { get; set; }
}