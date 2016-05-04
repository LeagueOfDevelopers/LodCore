using System;
using System.Text.RegularExpressions;

namespace Common
{
    public class TokenGenerator
    {
        public static string GenerateToken()
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            var str = Convert
                .ToBase64String(Guid.NewGuid().ToByteArray());
            return rgx.Replace(str, "");
        }
    }
}