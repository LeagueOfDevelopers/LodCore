using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LodCore.Domain.ProjectManagment;
using LodCoreApi.Models;
using Newtonsoft.Json;
using Image = LodCore.Common.Image;

namespace IntegrationControllerTests.Helpers
{
    public static class RequestHelper
    {
        public static HttpClient Client => new HttpClient();

        public static async Task<HttpResponseMessage> LoginToAdminAsync()
        {
            var account = Settings.ExistantAdminAccount;
            var credentials = new Credentials
            {
                Email = account.Email.Address,
                Password = account.Password.Value
            };

            var content = JsonConvert.SerializeObject(credentials);
            var responseMessage = await Client.PostAsync(
                GetEndpointAddress("login"),
                new StringContent(content) {Headers = {ContentType = new MediaTypeHeaderValue(@"text/json")}}
            );

            return responseMessage;
        }

        public static async Task<HttpResponseMessage> CreateProjectAsync()
        {
            var client = new HttpClient();
            var project = new ProjectActionRequest
            {
                Name = "TypicalNamsdfse",
                Info = "TypicalInfo",
                LandingImage = new Image(
                    new Uri("https://pp.vk.me/c543107/v543107881/af01/zxFX1YLyVOE.jpg"),
                    new Uri("https://pp.vk.me/c543107/v543107881/af01/zxFX1YLyVOE.jpg")),
                ProjectTypes = new[] {ProjectType.Game, ProjectType.MobileApp}
            };
            var response =
                await client.PostAsync(
                    new Uri(Settings.EndpointUri, "projects").ToString(),
                    project,
                    new JsonMediaTypeFormatter());
            return response;
        }

        public static Uri GetEndpointAddress(string address)
        {
            return new Uri(Settings.EndpointUri, address);
        }
    }
}