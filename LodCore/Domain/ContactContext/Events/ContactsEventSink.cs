﻿using Journalist;
using NotificationService;

namespace ContactContext.Events
{
    public class ContactsEventSink : EventSinkBase
    {
        public ContactsEventSink(
            IDistributionPolicyFactory distributionPolicyFactory, 
            IEventRepository eventRepository, 
            IMailer mailer) : base(distributionPolicyFactory, eventRepository, mailer)
        {
        }

        public override void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = DistributionPolicyFactory.GetAdminRelatedPolicy();

            EventRepository.DistrubuteEvent(@event, distributionPolicy);
        }
    }
}