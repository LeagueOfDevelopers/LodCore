using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LodCoreApi.Extensions
{
    public static class JwtTokensAnalyzer
    {
        public static int GetUserId(this HttpRequest request)
        {
            var token = request.Headers["Authorization"].ToString();

            var handler = new JwtSecurityTokenHandler();
            var userId =
                int.Parse(handler.ReadJwtToken(token.Substring(7)).Claims.First(c => c.Type == "UserId").Value);
            return userId;
        }

        public static bool IsInRole(this HttpRequest request, string role)
        {
            var token = request.Headers["Authorization"].ToString();

            if (token != "")
            {
                var handler = new JwtSecurityTokenHandler();
                var userRole = handler.ReadJwtToken(token.Substring(7)).Claims.First(c => c.Type == "Role").Value;

                return userRole == "Admin" || userRole == role;
            }

            return false;
        }
    }
}