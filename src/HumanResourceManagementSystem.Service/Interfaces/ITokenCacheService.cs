using Azure.Identity;
using HumanResourceManagementSystem.Service.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface ITokenCacheService
    {
        Task<bool> SetRefreshTokenAsync(string token, TokenCacheDto cacheData, TimeSpan expiry);
        Task<TokenCacheDto?> GetRefreshTokenAsync(string token);
        Task<bool> RemoveRefreshTokenAsync(string token);
        Task<bool> IsRefreshTokenValidAsync(string token);
        Task<bool> SetAsync(string key, string value, TimeSpan expiry);
        Task<bool> ExistsAsync(string key);
        Task<bool> RemoveAsync(string key);
    }
}
