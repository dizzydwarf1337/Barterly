using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Queries.Public.Posts.GetPostImages;

public class GetPostImagesCommandHandler : IRequestHandler<GetPostImagesCommand, ApiResponse<ICollection<PostImageDto>>>
{
    private readonly IMapper _mapper;
    private readonly IPostImageQueryRepository _postImageQueryRepository;

    public GetPostImagesCommandHandler(IPostImageQueryRepository postImageQueryRepository, IMapper mapper)
    {
        _mapper = mapper;
        _postImageQueryRepository = postImageQueryRepository;
    }

    public async Task<ApiResponse<ICollection<PostImageDto>>> Handle(GetPostImagesCommand request,
        CancellationToken cancellationToken)
    {
        var postImages =
            await _postImageQueryRepository.GetPostImagesByPostIdAsync(request.PostId, cancellationToken);
        return ApiResponse<ICollection<PostImageDto>>.Success(_mapper.Map<ICollection<PostImageDto>>(postImages));
    }
}