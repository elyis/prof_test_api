using System.Security.Claims;
using prof_tester_api.src.Domain.Entities.Shared;

namespace webApiTemplate.src.App.IService
{
    public interface IJwtService
    {
        string GenerateAccessToken(Dictionary<string, string> claims, TimeSpan timeSpan);
        string GenerateRefreshToken() => Guid.NewGuid().ToString();
        TokenPair GenerateDefaultTokenPair(Guid userId, Guid organizationId, string rolename);
        TokenPair GenerateTokenPair(Dictionary<string, string> claims, TimeSpan timeSpan);
        List<Claim> GetClaims(string token);
        TokenPayload GetTokenPayload(string token);
    }
}