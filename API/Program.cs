using API.Core.ServicesConfiguration;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Database;
using Persistence.Seed;
using System;
using System.IdentityModel.Tokens.Jwt;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using System.Text;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers(opt=>opt.Filters.Add<ValidationFilter>());


builder.Services.AddDbContext<BarterlyDbContext>(opt =>
    opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=Barterly;Integrated Security=True;TrustServerCertificate=True;")
);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
    });
});
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BarterlyDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("User", policy => policy.RequireRole("User"));
    opt.AddPolicy("Moderator", policy => policy.RequireRole("Moderator"));
    opt.AddPolicy("All", policy => policy.RequireRole("Moderator", "User", "Admin"));
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = jwtSettings["Key"];
var googleSettings = builder.Configuration.GetSection("GoogleAPI");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.AutomaticRefreshInterval = TimeSpan.FromDays(10);
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"Challenge issued with error: {context.Error}, description: {context.ErrorDescription}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var token = context.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwt = handler.ReadJwtToken(token);

                Console.WriteLine($"Token validated: {jwt.RawData}");
                Console.WriteLine($"Claims: {string.Join(", ", jwt.Claims.Select(c => $"{c.Type}: {c.Value}"))}");
                Console.WriteLine($"Valid To (UTC): {jwt.ValidTo}");
            }
            else
            {
                Console.WriteLine("Cannot read token");
            }


            return Task.CompletedTask;
        }
    };
})
.AddGoogle(options =>
{
    options.ClientId = googleSettings["ClientId"];
    options.ClientSecret = googleSettings["Key"];
    options.CallbackPath = "/signedin-google";
}); ;

ServiceConfig.ConfigureServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    string[] roleNames = { "Admin", "User", "Moderator" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
    await Seed.SeedDb(scope.ServiceProvider);
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



app.Run();

