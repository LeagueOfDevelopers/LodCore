using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class NewIssueEventType : EventType
    {
        public override string Type { get; }
    }
}