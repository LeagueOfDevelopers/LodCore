using System.Linq;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationService;
using UserManagement.Domain.Events;

namespace DataAccessTests
{
    [TestClass]
    public class EventRepositoryTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void EventIsStoredSuccessfully()
        {
            var provider = new DatabaseSessionProvider();
            provider.OpenSession();
            var repository = new EventRepository(provider);
            var eventInfo = new NewEmailConfirmedDeveloper(1);
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