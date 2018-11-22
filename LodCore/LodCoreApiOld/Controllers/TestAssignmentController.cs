using System;
using System.Linq;
using System.Web.Http;

namespace LodCoreApiOld.Controllers
{
    public class TestAssignmentController : ApiController
    {
        public string Get()
        {
            var currentTimeInMinutes = DateTime.UtcNow.Minute;
            var eventsCount = new Random(currentTimeInMinutes).Next(0, 4);
            var statisticEvents = Enumerable
                .Range(0, eventsCount)
                .Select(_ => new StatisticEvent(currentTimeInMinutes));
            var statisticsEventString = statisticEvents.Select(@event => $"{@event.Gender}:{@event.Condition}");
            return
                statisticsEventString.Any()
                    ? statisticsEventString
                        .Aggregate((@event1, @event2) => $"{@event1};{@event2}")
                    : string.Empty;
        }

        private class StatisticEvent
        {
            private static readonly string[] genders = {"Male", "Female"};

            private static readonly string[] conditions = {"Born", "Died"};

            public StatisticEvent(int seed)
            {
                var randomGenerator = new Random(seed);
                Gender = genders.OrderBy(gender => randomGenerator.Next(0, 4)).FirstOrDefault();
                Condition = conditions.OrderBy(condition => randomGenerator.Next(0, 4)).FirstOrDefault();
            }

            public string Gender { get; }

            public string Condition { get; }
        }
    }
}