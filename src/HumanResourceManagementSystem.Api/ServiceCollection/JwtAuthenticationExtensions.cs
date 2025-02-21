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
    public static class JwtAuthenticationExtensions
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
                });
            return services;
        }
    }
}
