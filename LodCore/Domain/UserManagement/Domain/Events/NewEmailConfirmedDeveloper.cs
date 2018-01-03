using Common;
using Journalist;
using RabbitMQEventBus;

namespace UserManagement.Domain.Events
{
    public class NewEmailConfirmedDeveloper : EventInfoBase
    {
        public NewEmailConfirmedDeveloper(int userId)
        {
            Require.Positive(userId, nameof(userId));

            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}