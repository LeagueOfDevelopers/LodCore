using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FrontendServices.Models;
using IntegrationControllerTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IntegrationControllerTests
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public async Task LoginToExistantAccountSucceeds()
        {
            var account = Settings.ExistantAdminAccount;
            var credentials = new Credentials
            {
                Email = account.Email.Address,
                Password = account.Password.Value
            };

            var content = JsonConvert.SerializeObject(credentials);
            var responseMessage = await RequestHelper.Client.PostAsync(
                RequestHelper.GetEndpointAddress("login"),
                new StringContent(content) { Headers = { ContentType = new MediaTypeHeaderValue(@"text/json")}} 
                );

            Assert.IsTrue(responseMessage.IsSuccessStatusCode);
        }
    }
}
