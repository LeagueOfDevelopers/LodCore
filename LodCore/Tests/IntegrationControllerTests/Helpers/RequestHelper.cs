using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FrontendServices.Models;
using Newtonsoft.Json;

namespace IntegrationControllerTests.Helpers
{
    public static class RequestHelper
    {
        public static async Task<HttpResponseMessage> LoginToAdminAsync()
        {
            var account = Settings.ExistantAdminAccount;
            var credentials = new Credentials
            {
                Email = account.Email.Address,
                Password = account.Password.Value
            };

            var content = JsonConvert.SerializeObject(credentials);
            var responseMessage = await RequestHelper.Client.PostAsync(
                GetEndpointAddress("login"),
                new StringContent(content) { Headers = { ContentType = new MediaTypeHeaderValue(@"text/json") } }
                );

            return responseMessage;
        }

        public static HttpClient Client => new HttpClient();

        public static Uri GetEndpointAddress(string address)
        {
            return new Uri(Settings.EndpointUri, address);
        }
    }
}