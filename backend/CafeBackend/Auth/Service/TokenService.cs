using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CafeBackend.Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace CafeBackend.Auth.Service
{
    
    public class TokenService
    {
        //readonly
        private readonly IConfiguration _config;

        private readonly string _secretKey;

        private readonly string _issuer;

        private readonly string _audience;

        private readonly int _expiryMin;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _secretKey = _config["JwtSettings:SecretKey"];
            _issuer = _config["JwtSettings:Issuer"];
            _audience = _config["JwtSettings:Audience"];
            _expiryMin = _config.GetValue<int>("JwtSetting:ExpiryMinutes",60);
        }

        //Create the Jwt
        public string GenerateToken(User user)
        {
            {
                var claims =new []
                {
                    new Claim("UserId",user.Id.ToString()), // custom claim
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role, user.Role), //Standard
                };

                //signing the key and cred
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                

                //create the new token
                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires : DateTime.UtcNow.AddMinutes(_expiryMin),
                    signingCredentials: cred

                );

                //return the newly created token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }

}