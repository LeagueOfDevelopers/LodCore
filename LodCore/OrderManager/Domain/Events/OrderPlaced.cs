using Journalist;
using NotificationService;

namespace OrderManagement.Domain.Events
{
    public class OrderPlaced : EventInfoBase
    {
        public OrderPlaced(int orderId)
        {
            Require.Positive(orderId, nameof(orderId));

            UserId = orderId;
        }

        public int UserId { get; private set; }
    }
}