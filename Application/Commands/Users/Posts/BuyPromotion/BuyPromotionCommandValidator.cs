using Domain.Enums.Posts;
using FluentValidation;

namespace Application.Commands.Users.Posts.BuyPromotion;

public class BuyPromotionCommandValidator : AbstractValidator<BuyPromotionCommand>
{
    public BuyPromotionCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty();
        RuleFor(x => x.PostPromotionType).Must(Enum.IsDefined);
    }
}