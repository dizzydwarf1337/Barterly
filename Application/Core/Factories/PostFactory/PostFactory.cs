using Application.Core.Factories.Interfaces;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Exceptions.DataExceptions;


namespace Application.Core.Factories.PostFactory
{
    public class PostFactory : IPostFactory
    {
        private readonly IMapper _mapper;

        public PostFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Post CreatePost(CreatePostDto createPostDto)
        {
            Post post;
            switch (createPostDto.PostType)
            {
                case "Rent": 
                    {
                        post = _mapper.Map<RentPost>(createPostDto);
                        break;
                    }
                case "Work": 
                    {
                        post = _mapper.Map<WorkPost>(createPostDto);
                        break;
                    }
                case "Common":
                    {
                        post = _mapper.Map<CommonPost>(createPostDto);
                        break;
                    }
                default: throw new InvalidDataProvidedException("PostType","PostDto","PostFactory.CreatePost");
            }
            //Promotion 
            var postPromotion = new Promotion
            {
                PostId = post.Id,
                Post = post,
            };


            //Settings
            var postSettings = new PostSettings
            {
                PostId = post.Id,
                Post = post,
            };
            post.PostSettingsId = postSettings.PostId;
            post.PostSettings = postSettings;
            post.PromotionId = postPromotion.Id;
            post.Promotion = postPromotion;
            return post;
        }
    }
}
