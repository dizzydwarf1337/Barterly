using Microsoft.EntityFrameworkCore;
using Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddDbContext<BarterlyDbContext>(opt =>
    opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=Barterly;Integrated Security=True;TrustServerCertificate=True;")
);
var app = builder.Build();


app.UseHttpsRedirection();


app.Run();

