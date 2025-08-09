using Application.Core.Behaviors;
using Application.Core.Factories.Interfaces;
using Application.Core.Factories.OpinionFactory;
using Application.Core.Factories.PostFactory;
using Application.Core.Factories.ReportFactory;
using Application.Core.Factories.UserFactory;
using Application.Core.Mapper;

namespace API.Core.ServicesConfiguration.Infrastructure;

public static class InfrastructureConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPostFactory, PostFactory>();
        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IOpinionFactory, OpinionFactory>();
        services.AddScoped<IReportFactory, ReportFactory>();
        services.AddAutoMapper(typeof(AutoMapperProfiler).Assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Application"));
            cfg.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            cfg.AddOpenBehavior(typeof(MessagesValidationBehavior<,>));

        });
        return services;
    }
}