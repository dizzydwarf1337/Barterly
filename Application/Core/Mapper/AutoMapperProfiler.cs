using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
            CreateMap<Post, PostDto>();
            CreateMap<ReportUser, ReportDto>().ForMember(x => x.RepotedSubjectId, opt => opt.MapFrom(x => x.ReportedUserId));
            CreateMap<ReportPost, ReportDto>().ForMember(x=>x.RepotedSubjectId,opt=>opt.MapFrom(x=>x.ReportedPostId));
            CreateMap<Category, CategoryDto>().ForMember(x=>x.SubCateogries,opt=> opt.MapFrom(x=>x.SubCategories));
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.token, opt => opt.Ignore());
            CreateMap<UserSettings,UserSettingsDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<UserOpinion,OpinionDto>().ForMember(x=>x.SubjectId, opt => opt.MapFrom(x => x.UserId));
            CreateMap<PostOpinion, OpinionDto>().ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.PostId));
        }
    }
}
