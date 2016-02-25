using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Common;
using FrontendServices.Models;
using IntegrationControllerTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using ProjectManagement.Domain;

namespace IntegrationControllerTests
{
    [TestClass]
    public class ProjectsTest
    {
        [TestMethod]
        public async Task ProperProjectCreatedSuccessfully()
        {
            var client = new HttpClient();
            var project = new CreateProjectRequest()
            {
                Name = "TypicalName",
                AccessLevel = AccessLevel.Public,
                Info = "TypicalInfo",
                LandingImageUri = new Uri("https://pp.vk.me/c543107/v543107881/af01/zxFX1YLyVOE.jpg"),
                ProjectTypes = new [] {ProjectType.Game, ProjectType.MobileApp, }
            };
            var response =
                await client.PostAsync(
                    new Uri(Settings.EndpointUri, "projects"), 
                    project, 
                    new JsonMediaTypeFormatter());

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        
        private Fixture Fixture => new Fixture(); 
    }
}