using System;

namespace NotificationService
{
    public interface IDomainEvent
    {
        DateTimeOffset CreatedAt { get; }
    }
}
