// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HumanResourceManagementSystem.Api.ServiceCollection
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisCache(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 註冊 RedisSettings
            services.Configure<RedisSettings>(
                configuration.GetSection("Redis"));

            // 註冊 IConnectionMultiplexer 為 Singleton
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var options = ConfigurationOptions.Parse(redisSettings.ConnectionString);

                // 可以在這裡加入額外的 Redis 配置
                options.AbortOnConnectFail = false;
                options.ConnectRetry = 3;

                return ConnectionMultiplexer.Connect(options);
            });

            // 註冊 ITokenCacheService
            services.AddScoped<ITokenCacheService, RedisTokenCacheService>();

            return services;
        }
    }
}
