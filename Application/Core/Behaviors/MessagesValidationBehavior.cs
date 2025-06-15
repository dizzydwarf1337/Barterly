using Application.Core.Validators.Interfaces;
using Application.Interfaces.CommandInterfaces;
using MediatR;


namespace Application.Core.Behaviors
{
    public class MessagesValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IMessageProfanityValidator _messageProfanityValidator;

        public MessagesValidationBehavior(IMessageProfanityValidator messageProfanityValidator)
        {
            _messageProfanityValidator = messageProfanityValidator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IMessageContainer message)
            {
                if (string.IsNullOrWhiteSpace(message.Message))
                {
                    throw new ArgumentNullException("Message cannot be null or empty.");
                }
                message.Message = _messageProfanityValidator.Censore(message.Message);
            }
            return await next();
        }
    }
}
