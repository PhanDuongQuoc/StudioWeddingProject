using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.Models;
using StudioWeddingServer.Services.UserPageServices.HomeServices;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
// Sign In Dependency Inject for HomePage 
builder.Services.AddScoped<IHomeService, HomeService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<StudioWeddingDbContext>(options =>
    options.UseNpgsql(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();