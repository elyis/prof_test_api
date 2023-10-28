using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using prof_tester_api.src.Domain.Entities.Shared;
using Microsoft.IdentityModel.Tokens;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.Domain.Entities.Config;

namespace webApiTemplate.src.App.Service
{
    public class JwtService : IJwtService
    {
        private readonly SigningCredentials _signingCredentials;

        public JwtService(JwtSettings jwtSettings)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateAccessToken(Dictionary<string, string> claims, TimeSpan timeSpan)
        {
            var tokenClaims = claims.Select(claim => new Claim(claim.Key, claim.Value));

            var token = new JwtSecurityToken(
                claims: tokenClaims,
                expires: DateTime.UtcNow.Add(timeSpan),
                signingCredentials: _signingCredentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken() => Guid.NewGuid().ToString();

        public TokenPair GenerateDefaultTokenPair(Guid userId, Guid organizationId, string rolename)
        {
            var claims = new Dictionary<string, string>{
                { "UserId", userId.ToString() },
                { ClaimTypes.Role, rolename},
                { "OrganizationId", organizationId.ToString()}
            };
            var timeSpan = new TimeSpan(2, 0, 0, 0);

            return GenerateTokenPair(claims, timeSpan);
        }

        public TokenPair GenerateTokenPair(Dictionary<string, string> claims, TimeSpan timeSpan) =>
            new TokenPair(
                    GenerateAccessToken(claims, timeSpan),
                    GenerateRefreshToken()
                );

        public List<Claim> GetClaims(string token) =>
            new JwtSecurityTokenHandler()
                .ReadJwtToken(token.Replace("Bearer ", ""))
                .Claims
                .ToList();

        public TokenPayload GetTokenPayload(string token)
        {
            var claims = GetClaims(token);
            var userId = claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var organizationId = claims.FirstOrDefault(claim => claim.Type == "OrganizationId")?.Value;

            return new TokenPayload
            {
                OrganizationId = Guid.Parse(organizationId),
                UserId = Guid.Parse(userId)
            };
        }
    }
}