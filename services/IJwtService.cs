using E_commerce_System.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_commerce_System.services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }


    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration configuration) => _config = configuration;
        
        public string GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
            };

            var encodedKey = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var key = new SymmetricSecurityKey(encodedKey);

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                signingCredentials:credentials
                );


            var tokenstr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenstr;
        
        }
    }
}
