namespace BuildingBlock.Security.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(Guid userId, string userName, IEnumerable<Guid> roles);
    }
}
