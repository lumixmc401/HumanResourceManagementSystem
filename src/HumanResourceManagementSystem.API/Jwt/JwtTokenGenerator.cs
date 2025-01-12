using HumanResourceManagementSystem.API.Models.Dto.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HumanResourceManagementSystem.API.Jwt
{
    public class JwtTokenGenerator
    {
        private readonly IOptions<JwtSettings> _jwtSettings;
        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
        }
        public string GenerateJwtToken(Guid userId, string userName, string[] roles)
        {
            if (string.IsNullOrEmpty(_jwtSettings.Value.SignKey))
                throw new InvalidOperationException("SignKey is not configured.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SignKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
            };

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claims.AddRange(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
