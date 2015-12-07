using Journalist;
using NotificationService;

namespace OrderManagement.Domain.Events
{
    public class OrderPlaced : EventInfoBase
    {
        public OrderPlaced(int orderId)
        {
            Require.Positive(orderId, nameof(orderId));

            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}