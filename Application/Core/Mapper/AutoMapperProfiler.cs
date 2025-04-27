using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Categories;
using Application.DTOs.General;
using Application.DTOs.Posts;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;

namespace Application.Core.Mapper
{
    public class AutoMapperProfiler : Profile
    {
        public AutoMapperProfiler()
        {
            CreateMap<GlobalNotification, NotificationDto>();
            CreateMap<Notification, NotificationDto>();
            CreateMap<Log, LogDto>();
            CreateMap<LogDto, Log>();
            CreateMap<Post, PostDto>()
                .ForMember(x=>x.Promotion, opt=>opt.MapFrom(x=>x.Promotion))
                .ForMember(x=>x.PostSettings, opt=>opt.MapFrom(x=>x.PostSettings))
                .ForMember(x=>x.PostOpinions, opt=>opt.MapFrom(x=>x.PostOpinions))
                .ForMember(x=>x.PostImages, opt=>opt.MapFrom(x=>x.PostImages))
                .ForMember(x=>x.SubCategory,opt=>opt.MapFrom(x=>x.SubCategory));
            CreateMap<WorkPost, PostDto>().IncludeBase<Post,PostDto>();
            CreateMap<RentPost, PostDto>().IncludeBase<Post, PostDto>();
            CreateMap<CommonPost, PostDto>().IncludeBase<Post, PostDto>();
            CreateMap<PostSettings, PostSettingsDto>();
            CreateMap<Post, PostPreviewDto>().ForMember(x=>x.PostPromotionType, opt=>opt.MapFrom(x=>x.Promotion.Type));
            CreateMap<PostPreviewDto, Post>();
            CreateMap<PostImage, PostImageDto>();
            CreateMap<Promotion, PromotionDto>();
            CreateMap<PostOpinion, PostOpinionDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<CreatePostDto, RentPost>().IncludeBase<CreatePostDto, Post>(); 
            CreateMap<CreatePostDto, WorkPost>().IncludeBase<CreatePostDto, Post>();  
            CreateMap<CreatePostDto, CommonPost>().IncludeBase<CreatePostDto, Post>();
            CreateMap<ReportUser, ReportDto>().ForMember(x => x.RepotedSubjectId, opt => opt.MapFrom(x => x.ReportedUserId));
            CreateMap<ReportPost, ReportDto>().ForMember(x => x.RepotedSubjectId, opt => opt.MapFrom(x => x.ReportedPostId));
            CreateMap<Category, CategoryDto>().ForMember(x => x.SubCategories, opt => opt.MapFrom(x => x.SubCategories));
            CreateMap<CategoryDto, Category>();
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<SubCategoryDto, SubCategory>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.token, opt => opt.Ignore());
            CreateMap<UserSettings, UserSettingsDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<UserOpinion, OpinionDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.UserId));
            CreateMap<PostOpinion, OpinionDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.PostId));
        }
    }
}
