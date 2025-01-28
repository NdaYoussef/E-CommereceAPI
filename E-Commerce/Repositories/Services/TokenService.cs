using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestToken.Models;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
             _configuration = configuration;
        }

        public RefreshToken GenerateRefreshToken()
        {
          var RandomNumber = new byte[32];
          var rng = RandomNumberGenerator.Create();
          rng.GetBytes(RandomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(1)
            };
        }
        
        public string GenerateToken(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.UserName!),
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims:Claims,
                signingCredentials : credentials,
                expires: DateTime.UtcNow.AddHours(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
