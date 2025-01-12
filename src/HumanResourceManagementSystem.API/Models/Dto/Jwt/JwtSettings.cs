namespace HumanResourceManagementSystem.API.Models.Dto.Jwt
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string SignKey { get; set; } = "";
    }
}
