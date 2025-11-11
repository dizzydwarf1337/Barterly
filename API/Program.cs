using API.Core.Extensions.Auth;
using API.Core.Extensions.Cors;
using API.Core.Extensions.Identity;
using API.Core.Extensions.Middleware;
using API.Core.Extensions.Persistence;
using API.Core.ServicesConfiguration.Commands;
using API.Core.ServicesConfiguration.Infrastructure;
using API.Core.ServicesConfiguration.Queries;
using API.Core.ServicesConfiguration.Services;
using Application.Hub;
using Microsoft.AspNetCore.Identity;
using Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAppServices()
    .AddDatabase(builder.Configuration)
    .AddIdentityServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddGoogleAuthentication(builder.Configuration)
    .AddAppAuthorization()
    .AddCorsPolicy()
    .AddGeneralCommands()
    .AddPostCommands()
    .AddUserCommands()
    .AddInfrastructure()
    .AddGeneralQueries()
    .AddPostQueries()
    .AddUserQueries()
    .AddApplicationServices()
    .AddValidators();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    string[] roleNames = { "Admin", "User", "Moderator" };
    foreach (var roleName in roleNames)
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

    await Seed.SeedDb(scope.ServiceProvider);
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.UseStaticFiles();


app.Run();