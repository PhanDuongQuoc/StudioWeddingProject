using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;
using StudioWeddingServer.Models;
using StudioWeddingServer.Services.AuthServices.LoginServices;
using StudioWeddingServer.Services.AuthServices.RegisterServices;
using StudioWeddingServer.Services.EmailServices;
using StudioWeddingServer.Services.ForgotServices;
using StudioWeddingServer.Services.UserPageServices.AboutServices;
using StudioWeddingServer.Services.UserPageServices.AlbumServices;
using StudioWeddingServer.Services.UserPageServices.EmailServices;
using StudioWeddingServer.Services.UserPageServices.HomeServices;
using StudioWeddingServer.Services.UserPageServices.PackageServices;
using StudioWeddingServer.Services.UserPageServices.SendEmailServices;
using StudioWeddingServer.Services.UserPageServices.ServiceServices;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Đăng ký MemoryCache (dùng để lưu OTP tạm thời)
builder.Services.AddMemoryCache();

// Sign In Dependency Inject for HomePage 
builder.Services.AddScoped<IHomeService, HomeService>();
// Sign in Dependency Inject for AuthService
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
// Sign in JWT service for function login 
// Sign in AlbumService 
builder.Services.AddScoped<IAlbumService, AlbumService>();
// Sign in Configution EmailSettings from appsettings.json
builder.Services.Configure<EmailSettingDto>(builder.Configuration.GetSection("EmailSettings"));
// Sign in Configution EmailService & ForgotService
builder.Services.AddScoped<ISendEmailService, SendEmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IForgotService, ForgotService>();
// Sign in Configution Service Service 
builder.Services.AddScoped<IServiceService, ServiceService>();
// Sign in  PackageService
builder.Services.AddScoped<IPackageService, StudioWeddingServer.Services.UserPageServices.PackageServices.PackageService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<JwtService>();
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

// JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            )
        };
    });


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập JWT token. Ví dụ: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();