using Application.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.Events.Opinions.UserOpinionCreated;
using Application.Events.Posts.PostOpinionCreatedEvent;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using MediatR;

namespace Application.Commands.Users.Opinions.CreateOpinion;

public class
    CreateOpinionCommandHandler : IRequestHandler<CreateOpinionCommand, ApiResponse<CreateOpinionCommand.Result>>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IOpinionFactory _opinionFactory;

    public CreateOpinionCommandHandler(IOpinionFactory opinionFactory, IMediator mediator, IMapper mapper)
    {
        _opinionFactory = opinionFactory;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CreateOpinionCommand.Result>> Handle(CreateOpinionCommand request,
        CancellationToken cancellationToken)
    {
        var opinion = await _opinionFactory.CreateOpinionAsync(request.OwnerId, request.SubjectId, request.Message,
            request.Rate, request.OpinionType, cancellationToken);
        switch (request.OpinionType)
        {
            case "User":
                await _mediator.Publish(new UserOpinionCreatedEvent { opinion = (UserOpinion)opinion });
                break;
            case "Post":
                await _mediator.Publish(new PostOpinionCreatedEvent { PostOpinion = (PostOpinion)opinion });
                break;
        }

        return ApiResponse<CreateOpinionCommand.Result>.Success(
            new CreateOpinionCommand.Result(
                opinion.Id,
                opinion.AuthorId,
                request.SubjectId,
                opinion.Content,
                opinion.Rate ?? 1,
                opinion.IsHidden,
                opinion.CreatedAt,
                opinion.LastUpdatedAt), 201);
    }
}