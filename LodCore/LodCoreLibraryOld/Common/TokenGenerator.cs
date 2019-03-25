using System;
using System.Text.RegularExpressions;

namespace LodCoreLibraryOld.Common
{
    public class TokenGenerator
    {
        public static string GenerateToken()
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            var str = Convert
                .ToBase64String(Guid.NewGuid().ToByteArray());
            return rgx.Replace(str, "");
        }
    }
}