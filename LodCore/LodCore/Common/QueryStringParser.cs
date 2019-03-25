using System;
using System.Web;

namespace LodCore.Common
{
    public static class QueryStringParser
    {
        public static string ParseQueryStringToGetParameter(string queryString, string parameter)
        {
            // HttpParseException was removed
            if (!queryString.Contains(parameter))
                throw new Exception("Http parse exception");
            var @params = HttpUtility.ParseQueryString(queryString);
            return @params[parameter];
        }
    }
}