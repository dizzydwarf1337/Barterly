using Application.Core.Mapper;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Database;
using Persistence.Repositories.Commands.General;
using Persistence.Repositories.Queries.General;

namespace BarterlyIntegrationTests.Core;

public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<BarterlyDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        dbContext = new BarterlyDbContext(options);


        var logCommandRepo = new LogCommandRepository(dbContext);
        var logQueryRepo = new LogQueryRepository(dbContext);
        var tokenCommandRepo = new TokenCommandRepository(dbContext);
        var tokenQueryRepo = new TokenQueryRepository(dbContext);

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiler>()).CreateMapper();

        logService = new LogService(logCommandRepo, logQueryRepo, mapper);
        tokenService = new TokenService(logService, userManager, _configuration, tokenQueryRepo, tokenCommandRepo);
        authService = new AuthService(userManager, tokenService, logService, mapper, mediator, _configuration);
    }

    public BarterlyDbContext dbContext { get; }
    public ILogService logService { get; }
    public IAuthService authService { get; private set; }
    public ITokenService tokenService { get; }
    public IConfiguration _configuration { get; private set; }
    public IMediator mediator { get; private set; }
    public UserManager<User> userManager { get; private set; }

    public void Dispose()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}