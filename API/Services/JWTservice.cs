using Clips_api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clips_api.Services
{
    public class JWTservice
    {
        private readonly SymmetricSecurityKey _jwtkey;
        private readonly IConfiguration _config;
        public JWTservice(IConfiguration config)
        {
            _config = config;
            _jwtkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        }

        public string CreateJwt(User user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
            };


            var Credentials = new SigningCredentials(_jwtkey,SecurityAlgorithms.HmacSha256Signature);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject= new ClaimsIdentity(userClaims),
                Expires= DateTime.UtcNow.AddDays(int.Parse(_config["JWT:ExpiredInDays"])),
                SigningCredentials= Credentials,
                Issuer = _config["JWT:Issuer"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
