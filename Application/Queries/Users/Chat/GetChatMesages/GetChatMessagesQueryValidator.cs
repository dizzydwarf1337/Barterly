using FluentValidation;

namespace Application.Queries.Users.Chat.GetChatMesages;

public class GetChatMessagesQueryValidator : AbstractValidator<GetChatMessagesQuery>
{
    public GetChatMessagesQueryValidator()
    {
        RuleFor(x => x.ChatId).NotEmpty();
        RuleFor(x => x.Page).NotEmpty().Must(x => x > 0);
        RuleFor(x => x.PageSize).NotEmpty().Must(x => x > 0);
    }
}