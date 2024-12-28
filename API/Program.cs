using Microsoft.EntityFrameworkCore;
using Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddDbContext<BarterlyDbContext>(opt =>
    opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=SonseArt2;Integrated Security=True;TrustServerCertificate=True;")
);
var app = builder.Build();


app.UseHttpsRedirection();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
