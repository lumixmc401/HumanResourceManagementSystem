using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using HumanResourceManagementSystem.Api.Models.Response;
using HumanResourceManagementSystem.Api.Tests.Constants.Api;
using HumanResourceManagementSystem.Service.DTOs.User;

namespace HumanResourceManagementSystem.Api.Tests.Factory
{
    public static class TestClientFactory
    {
        /// <summary>
        /// 建立無認證的 HttpClient
        /// </summary>
        public static HttpClient CreateClient()
        {
            return TestSetup.GetClient();
        }

        /// <summary>
        /// 建立已認證的 HttpClient (包含 Bearer Token 和 Cookie)
        /// </summary>
        public static async Task<HttpClient> CreateAuthenticatedClient(string email, string password)
        {
            var client = CreateClient();
            var (accessToken, cookie) = await LoginAndGetCookie(client, email, password);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("Cookie", $"RefreshToken={cookie.Value}");

            return client;
        }

        private static async Task<(string accessToken, Cookie cookie)> LoginAndGetCookie(
            HttpClient client,
            string email,
            string password)
        {
            var credentials = new LoginCredentialsDto
            {
                Email = email,
                Password = password
            };

            var response = await client.PostAsJsonAsync(TokenEndpoints.Login, credentials);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>()
                ?? throw new NullReferenceException("登入時發生錯誤");
            var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault()
                ?? throw new NullReferenceException("無法取得 Set-Cookie 標頭");
            var cookie = new Cookie("RefreshToken", setCookieHeader.Split(';').First().Split('=').Last());

            return (content.AccessToken, cookie);
        }
    }
}
