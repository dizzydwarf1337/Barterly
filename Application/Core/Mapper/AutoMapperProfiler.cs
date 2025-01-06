using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            CreateMap<UserOpinion, OpinionDto>();
            CreateMap<PostOpinion, OpinionDto>();
            CreateMap<Post, PostDto>();
            CreateMap<ReportUser, ReportDto>();
            CreateMap<ReportPost, ReportDto>();
            CreateMap<Session, SessionDto>();
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<User, UserDto>();
        }
    }
}
