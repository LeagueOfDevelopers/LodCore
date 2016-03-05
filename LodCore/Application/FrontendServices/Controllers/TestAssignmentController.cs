using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Policy;
using System.Web.Http;

namespace FrontendServices.Controllers
{
    public class TestAssignmentController : ApiController
    {
        public string Get()
        {
            var eventsCount = new Random().Next(0, 4);
            var statisticEvents = Enumerable
                .Range(0, eventsCount)
                .Select(_ => new StatisticEvent());
            var statisticsEventString = statisticEvents.Select(@event => $"{@event.Gender}:{@event.Condition}");
            return
                statisticsEventString.Any()
                    ? statisticsEventString
                        .Aggregate((@event1, @event2) => $"{@event1};{@event2}")
                    : string.Empty;
        }

        private class StatisticEvent
        {
            public StatisticEvent()
            {
                Gender = genders.OrderBy(gender => Guid.NewGuid()).FirstOrDefault();
                Condition = conditions.OrderBy(condition => Guid.NewGuid()).FirstOrDefault();
            }

            public string Gender { get; private set; }

            public string Condition { get; private set; }

            private static string[] genders = { "Male", "Female" };

            private static string[] conditions = { "Born", "Died" };
        }
    }
}
