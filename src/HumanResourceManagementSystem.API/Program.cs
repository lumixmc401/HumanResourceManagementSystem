using BuildingBlock.Exceptions.Handler;
using BuildingBlock.Middlewares;
using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.Implementations;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ���U DbContext
builder.Services.AddDbContext<HumanResourceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���U UnitOfWork
builder.Services.AddScoped<IHumanResourceUnitOfWork, HumanResourceUnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// �۰����ξE��
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HumanResourceDbContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler(options => { });

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

app.UseAuthorization();

app.MapControllers();

app.Run();
