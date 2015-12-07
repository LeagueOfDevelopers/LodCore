using Journalist;
using NotificationService;

namespace OrderManagement.Domain.Events
{
    public class OrderPlaced : EventInfoBase
    {
        public OrderPlaced(int orderId, string header, string description)
        {
            Require.Positive(orderId, nameof(orderId));
            Require.NotEmpty(header, nameof(header));
            Require.NotEmpty(description, nameof(description));


            UserId = orderId;
        }

        public int UserId { get; private set; }
    }
}