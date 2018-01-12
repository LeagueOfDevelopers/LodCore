﻿using Common;
using Journalist;
using UserPresentaton;

namespace NotificationService
{
    public class ContactsEventSink<T> : EventSinkBase<T> where T : IEventInfo
    {
        public ContactsEventSink(
            IDistributionPolicyFactory distributionPolicyFactory, 
            IEventRepository eventRepository, 
            IMailer mailer, IUserPresentationProvider userPresentationProvider) : 
            base(distributionPolicyFactory, eventRepository, mailer, userPresentationProvider)
        {
        }

        public override void Consume(T eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = DistributionPolicyFactory.GetAdminRelatedPolicy();

            EventRepository.DistrubuteEvent(@event, distributionPolicy);

            Mailer.SendNotificationEmail(distributionPolicy.ReceiverIds, eventInfo);
        }
    }
}