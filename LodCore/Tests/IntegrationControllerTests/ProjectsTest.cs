using System.Threading.Tasks;
using IntegrationControllerTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using System.Net.Http;

namespace IntegrationControllerTests
{
    [TestClass]
    public class ProjectsTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task ProperProjectCreatedSuccessfully()
        {
            var response = await RequestHelper.CreateProjectAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        
        private Fixture Fixture => new Fixture(); 
    }
}