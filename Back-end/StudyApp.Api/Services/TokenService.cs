using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudyApp.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyApp.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public int GetExpiryMinutes()
        {
            return _config.GetSection("Jwt").GetValue<int>("ExpiryMinutes", 60);
        }

        public string CreateToken(ApplicationUser user, IEnumerable<string>? roles = null)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key not configured");
            var issuer = jwtSection.GetValue<string>("Issuer") ?? "StudyApp";
            var audience = jwtSection.GetValue<string>("Audience") ?? "StudyAppClient";
            var expiryMinutes = GetExpiryMinutes();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (roles != null)
            {
                foreach (var r in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, r));
                }
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
