﻿using Journalist;

namespace NotificationService
{
    public abstract class EventSink
    {
        protected EventSink(IEventRepository repository, IDistributionPolicyFactory distributionPolicyFactory)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(repository, nameof(repository));

            DistributionPolicyFactory = distributionPolicyFactory;
            _repository = repository;
        }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected void ConsumeEvent(Event @event)
        {
            Require.NotNull(@event, nameof(@event));

            _repository.DistrubuteEvent(@event);
        }

        private readonly IEventRepository _repository;
    }
}