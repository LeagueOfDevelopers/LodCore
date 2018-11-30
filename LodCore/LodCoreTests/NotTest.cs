using System.Threading.Tasks;
using IntegrationControllerTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LodCore.QueryService.Handlers;
using LodCore.QueryService.Queries.NotificationQuery;
namespace IntegrationControllerTests
{
    [TestClass]
    public class NotTest
    {
        [TestMethod]
        public void MyTestMethod()
        {
        string connect = "Server=localhost;Database=test;Uid=root@localhost;Pwd=ybrjkfq86";

        NotificationHandler notificationHandler = new NotificationHandler(connect);
            notificationHandler.Handle(new PageNotificationForDeveloperQuery(1,0,2));

        }
    }
}
