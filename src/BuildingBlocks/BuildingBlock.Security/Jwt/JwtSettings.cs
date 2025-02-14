namespace BuildingBlock.Security.Jwt
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SignKey { get; set; } = string.Empty;
    }
}
