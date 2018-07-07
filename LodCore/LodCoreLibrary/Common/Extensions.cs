using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LodCoreLibrary.Common
{
    public static class Extensions
    {
        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> accounts, int number)
        {
            return accounts.OrderBy(account => Guid.NewGuid()).Take(number);
        }

        public static bool Like(this string toSearch, string toFind)
        {
            return
                new Regex(
                    @"\A" +
                    new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch)
                        .Replace('_', '.')
                        .Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }

        public static bool Contains(string toSearch, string toFind)
        {
            return toSearch.ToLower().Contains(toFind.ToLower());
        }
    }
}