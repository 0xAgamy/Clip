using Clips.Application.Interfaces;
using Clips.Domain.Models;
using Clips.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clips.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwt;
        public JwtService(IOptions<JwtOptions> jwt) => _jwt = jwt.Value;

        public string CreateJwt(User user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name,user.UserName ?? string.Empty),
            };


            var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: userClaims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwt.LifetimeMinutes),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                SecurityAlgorithms.HmacSha256Signature));


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
