using API.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Features.Posts.Queries.GetPostImages
{
    public class GetPostImagesCommandHandler : IRequestHandler<GetPostImagesCommand, ApiResponse<PostImagesDto>>
    {
        private readonly IPostImageQueryRepository _postImageQueryRepository;
        private readonly IMapper _mapper;
        public GetPostImagesCommandHandler(IPostImageQueryRepository postImageQueryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _postImageQueryRepository = postImageQueryRepository;
        }

        public async Task<ApiResponse<PostImagesDto>> Handle(GetPostImagesCommand request, CancellationToken cancellationToken)
        {

                var postImages = await _postImageQueryRepository.GetPostImagesByPostIdAsync(Guid.Parse(request.PostId));
                return ApiResponse<PostImagesDto>.Success( _mapper.Map<PostImagesDto>(postImages));
     
        }
    }
}
