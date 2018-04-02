
using System.Web;

namespace Common
{
    public static class QueryStringParser
    {
        public static string ParseQueryStringToGetParameter(string queryString, string parameter)
        {
            if (!queryString.Contains(parameter))
                throw new HttpParseException();
            var @params = HttpUtility.ParseQueryString(queryString);
            return @params[parameter];
        }
    }
}
