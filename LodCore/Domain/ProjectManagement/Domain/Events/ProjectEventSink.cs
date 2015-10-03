using System;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class ProjectEventSink : AbstractEventSink
    {
        public ProjectEventSink(IEventRepository eventRepository) : base(eventRepository)
        {
        }

        public void SendNewIssueEvent(Issue issue, Project project)
        {
            var @event = new Event();
        }
    }
}