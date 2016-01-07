using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class Extensions
    {
        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> accounts, int number)
        {
            return accounts.OrderBy(account => Guid.NewGuid()).Take(number);
        }
    }
}