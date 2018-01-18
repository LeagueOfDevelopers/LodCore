﻿using Common;
using Journalist;
using ProjectManagement.Domain.Events;
using UserPresentaton;

namespace NotificationService
{
    public class ProjectsEventSink<T> : EventSinkBase<T> where T : IEventInfo
    {
        public ProjectsEventSink(IDistributionPolicyFactory distributionPolicyFactory,
                                 IEventRepository eventRepository, 
                                 IMailer mailer, 
                                 IUserPresentationProvider userPresentationProvider)
            : base(distributionPolicyFactory, eventRepository, mailer, userPresentationProvider)
        {
        }

        public override void Consume(T eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var distributionPolicy = GetDistributionPolicyForEvent((dynamic) eventInfo);

            EventRepository.SaveEvent(new Event(eventInfo), distributionPolicy);

            SendOutEmailsAboutEvent(distributionPolicy.ReceiverIds, eventInfo);
        }

        private DistributionPolicy GetDistributionPolicyForEvent(DeveloperHasLeftProject @eventInfo)
        {
            return
                DistributionPolicyFactory.GetProjectRelatedPolicy(@eventInfo.ProjectId)
                    .Merge(DistributionPolicyFactory.GetAdminRelatedPolicy());
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewDeveloperOnProject @eventInfo)
        {
            return
                DistributionPolicyFactory.GetProjectRelatedPolicy(@eventInfo.ProjectId)
                    .Merge(DistributionPolicyFactory.GetAdminRelatedPolicy());
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewProjectCreated @eventInfo)
        {
            return DistributionPolicyFactory.GetAllPolicy();
        }
    }
}