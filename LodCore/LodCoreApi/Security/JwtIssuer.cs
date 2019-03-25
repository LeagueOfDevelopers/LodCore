using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LodCoreApi.Security
{
    public class JwtIssuer : IJwtIssuer
    {
        private readonly SecuritySettings _securitySettings;

        public JwtIssuer(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }

        public string IssueJwt(string role, int id)
        {
            var claims = new[]
            {
                new Claim(Claims.Roles.RoleClaim, role),
                new Claim(Claims.IdClaim, id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securitySettings.EncryptionKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_securitySettings.Issue, claims: claims,
                expires: DateTime.Now.Add(_securitySettings.ExpirationPeriod),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}