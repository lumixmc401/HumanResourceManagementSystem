using DotNet.Testcontainers.Builders;
using HumanResourceManagementSystem.Api.Tests.Constants.Api;
using HumanResourceManagementSystem.Api.Tests.Data;
using HumanResourceManagementSystem.Api.Tests.Factories;
using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Service.DTOs.User;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace HumanResourceManagementSystem.Api.Tests
{
    [SetUpFixture]
    public class TestSetup
    {
        private static IServiceProvider? _serviceProvider;
        private static MsSqlContainer DbContainer;
        private static RedisContainer RedisContainer;
        private static HumanResourceManagementSystemWebApplicationFactory<Program> Factory;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            DbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithPortBinding(1433,true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();

            RedisContainer = new RedisBuilder()
                 .WithImage("redis:latest")
                 .WithPortBinding(6379, true)
                 .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
                 .Build();


            await DbContainer.StartAsync();
            await RedisContainer.StartAsync();
            Factory = new HumanResourceManagementSystemWebApplicationFactory<Program>(DbContainer,RedisContainer);
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HumanResourceDbContext>();
            _serviceProvider = Factory.Services;
            await dbContext.Database.MigrateAsync();
            await DatabaseSeeder.SeedTestData(dbContext);
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            await DbContainer.DisposeAsync();
            await RedisContainer.DisposeAsync();
            await Factory.DisposeAsync();
        }

        public static HttpClient GetClient()
        {
            return Factory.CreateClient();
        }

        public static T GetService<T>() where T : class
        {
            using var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
