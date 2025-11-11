using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Categories;
using Application.DTOs.General;
using Application.DTOs.General.Opinions;
using Application.DTOs.Posts;
using Application.DTOs.Reports;
using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;

namespace Application.Core.Mapper;

public class AutoMapperProfiler : Profile
{
    public AutoMapperProfiler()
    {
        CreateMap<GlobalNotification, NotificationDto>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<Log, LogDto>();
        CreateMap<LogDto, Log>();
        CreateMap<Post, PostDto>()
            .ForMember(x => x.PostImages, opt => opt.MapFrom(x => x.PostImages))
            .ForMember(x => x.SubCategory, opt => opt.MapFrom(x => x.SubCategory))
            .ForMember(x => x.PostType, opt => opt.MapFrom(x =>
                x is WorkPost ? "Work" :
                x is RentPost ? "Rent" :
                "Common"));
        CreateMap<WorkPost, PostDto>().IncludeBase<Post, PostDto>();
        CreateMap<RentPost, PostDto>().IncludeBase<Post, PostDto>();
        CreateMap<CommonPost, PostDto>().IncludeBase<Post, PostDto>();
        CreateMap<PostDto, Post>()
            .ForMember(x => x.PostImages, opt => opt.MapFrom(x => x.PostImages))
            .ForMember(x => x.SubCategory, opt => opt.MapFrom(x => x.SubCategory));

        CreateMap<EditPostDto, Post>()
            .ForMember(x => x.CreatedAt, opt => opt.Ignore());
        CreateMap<PostSettings, PostSettingsDto>();

        CreateMap<Post, PostPreviewDto>()
            .ForMember(x => x.PostPromotionType, opt => opt.MapFrom(x => x.Promotion.Type))
            .ForMember(x => x.PostType, opt => opt.MapFrom(x =>
                x is WorkPost ? "Work" :
                x is RentPost ? "Rent" :
                "Common"))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(y => y.CreatedAt))
            .ForMember(x => x.OwnerName, opt => opt.MapFrom(x => $"{x.Owner.FirstName} {x.Owner.LastName}"));
        CreateMap<WorkPost, PostPreviewDto>().IncludeBase<Post, PostPreviewDto>();
        CreateMap<RentPost, PostPreviewDto>().IncludeBase<Post, PostPreviewDto>();
        CreateMap<CommonPost, PostPreviewDto>().IncludeBase<Post, PostPreviewDto>();
        CreateMap<PostDto, Post>()
            .ForMember(x => x.PostImages, opt => opt.MapFrom(x => x.PostImages))
            .ForMember(x => x.SubCategory, opt => opt.MapFrom(x => x.SubCategory));

        CreateMap<PostPreviewDto, Post>();
        CreateMap<PostImage, PostImageDto>()
            .ForMember(x=>x.ImageUrl, opt => opt.MapFrom(x=>x.ImageUrl))
            .ForMember(x=>x.Id, opt=>opt.MapFrom(x=>x.Id));
        CreateMap<PostImageDto, PostImage>();
        CreateMap<Promotion, PromotionDto>();
        CreateMap<PostOpinion, PostOpinionDto>();
        CreateMap<CreatePostDto, Post>();
        CreateMap<CreatePostDto, RentPost>().IncludeBase<CreatePostDto, Post>();
        CreateMap<CreatePostDto, WorkPost>().IncludeBase<CreatePostDto, Post>();
        CreateMap<CreatePostDto, CommonPost>().IncludeBase<CreatePostDto, Post>();
        CreateMap<ReportUser, ReportDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.ReportedUserId))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt));
        CreateMap<ReportPost, ReportDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.ReportedPostId))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt));

        CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(x=>x.NameEN, opt => opt.MapFrom(y=>y.TitleEN))
            .ForMember(x=>x.NamePL, opt => opt.MapFrom(y=>y.TitlePL));
        CreateMap<SubCategoryDto, SubCategory>()
            .ForMember(x=>x.TitleEN, opt => opt.MapFrom(y=>y.NameEN))
            .ForMember(x=>x.TitlePL, opt => opt.MapFrom(y=> y.NamePL));
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.token, opt => opt.Ignore());
        CreateMap<UserSettings, UserSettingsDto>();
        CreateMap<RegisterDto, User>();
        CreateMap<User, RegisterDto>();
        CreateMap<UserOpinion, OpinionDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.UserId));
        CreateMap<PostOpinion, OpinionDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.PostId));

        CreateMap<OpinionDto, UserOpinion>()
            .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.SubjectId));
        CreateMap<OpinionDto, PostOpinion>()
            .ForMember(x => x.PostId, opt => opt.MapFrom(x => x.SubjectId));
        CreateMap<CreateOpinionDto, PostOpinion>()
            .ForMember(x => x.PostId, opt => opt.MapFrom(x => x.SubjectId));
        CreateMap<CreateOpinionDto, UserOpinion>()
            .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.SubjectId));
    }
}