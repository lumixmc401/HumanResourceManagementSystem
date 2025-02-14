using AngleSharp;
using BuildingBlock.Security.Jwt;
using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Service.DTOs.Token;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace HumanResourceManagementSystem.Api.Tests.Factory
{
    public class HumanResourceManagementSystemWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
         where TProgram : class
    {
        public HumanResourceManagementSystemWebApplicationFactory(MsSqlContainer dbContainer, RedisContainer redisContainer)
        {
            _dbContainer = dbContainer;
            _redisContainer = redisContainer;
        }
        private readonly MsSqlContainer _dbContainer;
        private readonly RedisContainer _redisContainer;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"JwtSettings:Issuer", "http://localhost"},
                    {"JwtSettings:SignKey", "ThisIshfkdjshfkdgfjsgfsdgfksdjfkahfjksjfkhsfhkdshkj"},
                    {"JwtSettings:Audience", "http://localhost"},
                    {"Redis:ConnectionString", _redisContainer.GetConnectionString()},
                    {"Redis:InstanceName", "TokenCache"},
                });
            });
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<HumanResourceDbContext>(options =>
                {
                    string cs = _dbContainer.GetConnectionString();
                    options.UseSqlServer(_dbContainer.GetConnectionString());
                });
            });
            builder.UseEnvironment("Development");
            base.ConfigureWebHost(builder);
        }
    }
}
