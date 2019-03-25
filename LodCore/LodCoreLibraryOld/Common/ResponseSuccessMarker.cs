using System;

namespace LodCoreLibraryOld.Common
{
    public static class ResponseSuccessMarker
    {
        public static Uri MarkRedirectUrlSuccessAs(string url, bool status)
        {
            if (url.Contains("?"))
                return new Uri($"{url}&success={status}");
            return new Uri($"{url}?success={status}");
        }
    }
}