using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using BuildingBlock.Security.Jwt;
using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HumanResourceManagementSystem.Api.ServiceCollection
{
    public static class JwtAuthenticationExtension
    {
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
                        ?? throw new ArgumentNullException(nameof(JwtSettings));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.SignKey ?? throw new ArgumentNullException("Jwt SignKey Not Set"))),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var tokenService = context.HttpContext.RequestServices
                                .GetRequiredService<ITokenService>();

                            var token = context.SecurityToken as JwtSecurityToken;
                            if (token != null)
                            {
                                var isBlacklisted = await tokenService
                                    .IsAccessTokenBlacklistedAsync(token.RawData);

                                if (isBlacklisted)
                                {
                                    context.Fail(new SecurityTokenException("Token has been blacklisted"));
                                }
                            }
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new
                            {
                                message = context.Exception.Message
                            });

                            return context.Response.WriteAsync(result);
                        }
                    };
                });
            return services;
        }
    }
}
