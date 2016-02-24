using System.Threading.Tasks;
using IntegrationControllerTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationControllerTests
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public async Task LoginToExistantAccountSucceeds()
        {
            var responseMessage = await RequestHelper.LoginToAdminAsync();

            Assert.IsTrue(responseMessage.IsSuccessStatusCode);
        }
    }
}
