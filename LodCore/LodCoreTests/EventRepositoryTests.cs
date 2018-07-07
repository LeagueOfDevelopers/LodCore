using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LodCoreLibrary.Infrastructure.DataAccess;
using LodCoreLibrary.Infrastructure.WebSocketConnection;
using LodCoreLibrary.Domain.NotificationService;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;

namespace DataAccessTests
{
    [TestClass]
    public class EventRepositoryTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void EventIsStoredSuccessfully()
        {
            var dbProvider = new DatabaseSessionProvider();
            dbProvider.OpenSession();
            var wsProvider = new WebSocketStreamProvider();
            var repository = new EventRepository(dbProvider, wsProvider);
            var eventInfo = new NewEmailConfirmedDeveloper(1, "firstName", "lastName");
            var @event = new Event(eventInfo);
            var receivers = new[] {30, 31, 32};
            var distributionPolicy = new DistributionPolicy(receivers);

            repository.SaveEvent(@event, distributionPolicy);
            var receivedEvent = repository.GetEventsByUser(receivers[0], false).Single();

            Assert.AreEqual(@event.EventInfo, receivedEvent.EventInfo);
            Assert.AreEqual(@event.EventType, receivedEvent.EventType);
        }
    }
}