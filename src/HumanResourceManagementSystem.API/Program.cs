using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Data.UnitOfWorks;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.Implementations;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊 DbContext
builder.Services.AddDbContext<HumanResourceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 註冊 UnitOfWork
builder.Services.AddScoped<IHumanResourceUnitOfWork, HumanResourceUnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// 自動應用遷移
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HumanResourceDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HumanResourceManagementSystem API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
