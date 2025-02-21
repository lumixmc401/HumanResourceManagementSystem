// src/HumanResourceManagementSystem.Service/Implementations/RedisTokenCacheService.cs
using HumanResourceManagementSystem.Service.DTOs.Token;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

public class RedisTokenCacheService : ITokenCacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly string _instanceName;
    private const string KeyPrefix = "refresh_token:";

    public RedisTokenCacheService(
        IConnectionMultiplexer redis,
        IOptions<RedisSettings> redisSettings)
    {
        _redis = redis;
        _instanceName = redisSettings.Value.InstanceName;
    }

    private IDatabase Database => _redis.GetDatabase();

    private string GetKey(string token) => $"{_instanceName}:{KeyPrefix}{token}";

    public async Task<bool> SetRefreshTokenAsync(string token, TokenCacheDto cacheData, TimeSpan expiry)
    {
        string key = GetKey(token);
        string value = JsonSerializer.Serialize(cacheData);
        return await Database.StringSetAsync(key, value, expiry);
    }

    public async Task<TokenCacheDto?> GetRefreshTokenAsync(string token)
    {
        string key = GetKey(token);
        var value = await Database.StringGetAsync(key);
        return value.HasValue 
            ? JsonSerializer.Deserialize<TokenCacheDto>(value!) 
            : null;
    }

    public async Task<bool> RemoveRefreshTokenAsync(string token)
    {
        string key = GetKey(token);
        return await Database.KeyDeleteAsync(key);
    }

    public async Task<bool> IsRefreshTokenValidAsync(string token)
    {
        string key = GetKey(token);
        return await Database.KeyExistsAsync(key);
    }

    public async Task<bool> SetAsync(string key, string value, TimeSpan expiry)
    {
        var db = _redis.GetDatabase();
        return await db.StringSetAsync(key, value, expiry);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.KeyExistsAsync(key);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }
}
