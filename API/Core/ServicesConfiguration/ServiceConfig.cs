using Application.Core.Factories.Interfaces;
using Application.Core.Factories.PostFactory;
using Application.Core.Factories.UserFactory;
using Application.Core.Mapper;
using Application.Core.Validators.Implementation;
using Application.Core.Validators.Interfaces;
using Application.DTOs.Categories;
using Application.Features.Category.Commands.AddCategory;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.General;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using FluentValidation;
using Persistence.Repositories.Commands.General;
using Persistence.Repositories.Commands.Post;
using Persistence.Repositories.Commands.Posts;
using Persistence.Repositories.Commands.Users;
using Persistence.Repositories.Queries.General;
using Persistence.Repositories.Queries.Post;
using Persistence.Repositories.Queries.Posts;
using Persistence.Repositories.Queries.Users;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;


namespace API.Core.ServicesConfiguration
{
    public class ServiceConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //General commands
            services.AddScoped<ILogCommandRepository, LogCommandRepository>();
            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<IGlobalNotificationCommandRepository, GlobalNotificationCommandRepository>();
            services.AddScoped<INotificationCommandRepository, NotificationCommandRepository>();
            services.AddScoped<ITokenCommandRepository, TokenCommandRepository>();

            //Post commands
            services.AddScoped<IPostOpinionCommandRepository, PostOpinionCommandRepository>();
            services.AddScoped<IReportPostCommandRepository, ReportPostCommandRepository>();
            services.AddScoped<IPostCommandRepository, PostCommandRepository>();
            services.AddScoped<IPostImageCommandRepository, PostImageCommandRepository>();
            services.AddScoped<IPromotionCommandRepository, PromotionCommandRepository>();
            services.AddScoped<IPostSettingsCommandRepository, PostSettingsCommandRepository>();

            //User commands
            services.AddScoped<IUserActivityCommandRepository, UserActivityCommandRepository>();
            services.AddScoped<IUserOpinionCommandRepository, UserOpinionCommandRepository>();
            services.AddScoped<IReportUserCommandRepository, ReportUserCommandRepository>();
            services.AddScoped<IVisitedPostCommandRepository, VisitedPostCommandRepository>();
            services.AddScoped<IUserSettingCommandRepository, UserSettingCommandRepository>();
            services.AddScoped<IUserCommandRepository, UserCommandRepository>();


            
            
            //General queries
            services.AddScoped<ILogQueryRepository, LogQueryRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
            services.AddScoped<IGlobalNotificationQueryRepository, GlobalNotificationQueryRepository>();
            services.AddScoped<INotificationQueryRepository, NotificationQueryRepository>();
            services.AddScoped<ITokenQueryRepository, TokenQueryRepository>();

            //Posts query
            services.AddScoped<IPostOpinionQueryRepository, PostOpinionQueryRepository>();
            services.AddScoped<IReportPostQueryRepository, ReportPostQueryRepository>();
            services.AddScoped<IPostQueryRepository, PostQueryRepository>();
            services.AddScoped<IPostImageQueryRepository, PostImageQueryRepository>();
            services.AddScoped<IPromotionQueryRepository, PromotionQueryRepository>();
            services.AddScoped<IPostSettingsQueryRepository, PostSettingsQueryRepository>();
            
            //Users query
            services.AddScoped<IUserActivityQueryRepository, UserActivityQueryRepository>();
            services.AddScoped<IUserOpinionQueryRepository, UserOpinionQueryRepository>();
            services.AddScoped<IUserSettingQueryRepository, UserSettingQueryRepository>();
            services.AddScoped<IReportUserQueryRepository, ReportUserQueryRepository>();
            services.AddScoped<IVisitedPostQueryRepository, VisitedPostQueryRepository>();
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();



            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMailService, GmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IPostOpinionService, PostOpinionService>();
            services.AddScoped<IPostReportService, PostReportService>();
            services.AddScoped<IPostSettingsService, PostSettingsService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPostFactory, PostFactory>();

            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IUserOpinionService, UserOpinionService>();
            services.AddScoped<IUserReportService, UserReportService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVisitingPostService, VisitingPostService>();
            services.AddScoped<IUserSettingsService, UserSettingsService>();
            services.AddScoped<IUserFactory, UserFactory>();
            
            services.AddScoped<IOwnershipValidator, OwnershipValidator>();
            
            

            services.AddAutoMapper(typeof(AutoMapperProfiler).Assembly);
            
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(AddCategoryCommand).Assembly);
            });






        }
    }
}
