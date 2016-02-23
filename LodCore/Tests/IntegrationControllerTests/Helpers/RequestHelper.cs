using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationControllerTests.Helpers
{
    public static class RequestHelper
    {
        public static HttpClient Client => new HttpClient();

        public static Uri GetEndpointAddress(string address)
        {
            return new Uri(Settings.EndpointUri, address);
        }
    }
}