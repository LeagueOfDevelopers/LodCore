using System;
using Journalist;

namespace NotificationService
{
    public abstract class Event
    {
        protected Event(DistributionPolicy distributionPolicy)
        {
            Require.NotNull(distributionPolicy, nameof(distributionPolicy));

            DistributionPolicy = distributionPolicy;

            Timestamp = DateTime.Now;
        }

        public int Id { get; set; }

        public DistributionPolicy DistributionPolicy { get; private set; }

        public DateTime Timestamp { get; private set; }

        public virtual string EventType => GetType().ToString();
    }
}