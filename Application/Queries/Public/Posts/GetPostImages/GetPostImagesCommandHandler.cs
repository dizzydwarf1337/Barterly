using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Queries.Public.Posts.GetPostImages;

public class GetPostImagesCommandHandler : IRequestHandler<GetPostImagesCommand, ApiResponse<PostImagesDto>>
{
    private readonly IMapper _mapper;
    private readonly IPostImageQueryRepository _postImageQueryRepository;

    public GetPostImagesCommandHandler(IPostImageQueryRepository postImageQueryRepository, IMapper mapper)
    {
        _mapper = mapper;
        _postImageQueryRepository = postImageQueryRepository;
    }

    public async Task<ApiResponse<PostImagesDto>> Handle(GetPostImagesCommand request,
        CancellationToken cancellationToken)
    {
        var postImages =
            await _postImageQueryRepository.GetPostImagesByPostIdAsync(request.PostId, cancellationToken);
        return ApiResponse<PostImagesDto>.Success(_mapper.Map<PostImagesDto>(postImages));
    }
}