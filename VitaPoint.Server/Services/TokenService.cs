using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        private readonly UserManager<Account> userManager;
        private readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration config, UserManager<Account> userManager)
        {
            this.config = config;
            this.userManager = userManager;
            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"]));
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> CreateToken(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Email)
            };

            var roles = await userManager.GetRolesAsync(account);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = config["JWT:Issuer"],
                Audience = config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
