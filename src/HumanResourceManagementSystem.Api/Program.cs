using BuildingBlock.Core.Common.Exceptions.Handler;
using HumanResourceManagementSystem.Api.ServiceCollection;
using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.Implementations;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using HumanResourceManagementSystem.Service.DTOs.Token;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.CookiePolicy;
using HumanResourceManagementSystem.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "HumanResourceManagementSystem API", Version = "v1" });

    // 加入 JWT Authentication 的設定
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddRedisCache(builder.Configuration);

// 註冊 DbContext
builder.Services.AddDbContext<HumanResourceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<ITokenCacheService, RedisTokenCacheService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// 註冊 UnitOfWork
builder.Services.AddScoped<IHumanResourceUnitOfWork, HumanResourceUnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplicationValidators();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});

var app = builder.Build();

// 自動應用遷移
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HumanResourceDbContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler(options => { });

app.UseCookiePolicy();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HumanResourceManagementSystem API V1");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();

app.UseAuthentication();
app.UseTokenBlacklistValidation();
app.UseAuthorization();

app.MapControllers();

app.Run();
