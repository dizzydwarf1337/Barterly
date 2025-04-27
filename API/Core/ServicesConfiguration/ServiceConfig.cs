using Application.Core.Factories.Interfaces;
using Application.Core.Factories.PostFactory;
using Application.Core.Mapper;
using Application.Features.Category.Commands.AddCategory;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.General;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using Persistence.Repositories.Commands.General;
using Persistence.Repositories.Commands.Post;
using Persistence.Repositories.Commands.Posts;
using Persistence.Repositories.Commands.Users;
using Persistence.Repositories.Queries.General;
using Persistence.Repositories.Queries.Post;
using Persistence.Repositories.Queries.Posts;
using Persistence.Repositories.Queries.Users;

namespace API.Core.ServicesConfiguration
{
    public class ServiceConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILogCommandRepository, LogCommandRepository>();
            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<IPostOpinionCommandRepository, PostOpinionCommandRepository>();
            services.AddScoped<IReportPostCommandRepository, ReportPostCommandRepository>();
            services.AddScoped<IUserActivityCommandRepository, UserActivityCommandRepository>();
            services.AddScoped<IUserOpinionCommandRepository, UserOpinionCommandRepository>();
            services.AddScoped<IReportUserCommandRepository, ReportUserCommandRepository>();
            services.AddScoped<IVisitedPostCommandRepository, VisitedPostCommandRepository>();
            services.AddScoped<IGlobalNotificationCommandRepository, GlobalNotificationCommandRepository>();
            services.AddScoped<INotificationCommandRepository, NotificationCommandRepository>();
            services.AddScoped<ITokenCommandRepository, TokenCommandRepository>();
            services.AddScoped<IPostCommandRepository, PostCommandRepository>();
            services.AddScoped<IPostImageCommandRepository, PostImageCommandRepository>();
            services.AddScoped<IPromotionCommandRepository, PromotionCommandRepository>();
            services.AddScoped<IUserSettingCommandRepository, UserSettingCommandRepository>();
            services.AddScoped<IPostSettingsCommandRepository, PostSettingsCommandRepository>();
            

            services.AddScoped<ILogQueryRepository, LogQueryRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
            services.AddScoped<IPostOpinionQueryRepository, PostOpinionQueryRepository>();
            services.AddScoped<IReportPostQueryRepository, ReportPostQueryRepository>();
            services.AddScoped<IUserActivityQueryRepository, UserActivityQueryRepository>();
            services.AddScoped<IUserOpinionQueryRepository, UserOpinionQueryRepository>();
            services.AddScoped<IReportUserQueryRepository, ReportUserQueryRepository>();
            services.AddScoped<IVisitedPostQueryRepository, VisitedPostQueryRepository>();
            services.AddScoped<IGlobalNotificationQueryRepository, GlobalNotificationQueryRepository>();
            services.AddScoped<INotificationQueryRepository, NotificationQueryRepository>();
            services.AddScoped<ITokenQueryRepository, TokenQueryRepository>();
            services.AddScoped<IPostQueryRepository, PostQueryRepository>();
            services.AddScoped<IPostImageQueryRepository, PostImageQueryRepository>();
            services.AddScoped<IPromotionQueryRepository, PromotionQueryRepository>();
            services.AddScoped<IUserSettingQueryRepository, UserSettingQueryRepository>();
            services.AddScoped<IPostSettingsQueryRepository, PostSettingsQueryRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMailService, GmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOpinionService, PostOpinionService>();
            services.AddScoped<IReportService, PostReportService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IOpinionService, UserOpinionService>();
            services.AddScoped<IReportService, UserReportService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVisitingPostService, VisitingPostService>();
            services.AddScoped<IUserSettingsService, UserSettingsService>();
            services.AddScoped<IPostFactory, PostFactory>();

            services.AddAutoMapper(typeof(AutoMapperProfiler).Assembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(AddCategoryCommand).Assembly);
            });


        }
    }
}
