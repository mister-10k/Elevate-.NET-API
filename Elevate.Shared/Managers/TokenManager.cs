using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public static class TokenManager
    {
        public static string GenerateToken(string email, int comapnyId)
        {
            byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["secretKey"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            var claims = new Dictionary<string, object>();
            claims.Add("companyId", comapnyId);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, email)
                }),
                Claims = claims,
                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static string GenerateSecretKey()
        {
            HMACSHA256 hmac = new HMACSHA256();
            string key = Convert.ToBase64String(hmac.Key);
            return key;
        }
    }
}
