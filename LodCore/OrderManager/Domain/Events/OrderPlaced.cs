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
            Header = header;
            Description = description;
        }

        public int UserId { get; private set; }
        public string Header { get; private set; }
        public string Description { get; private set; }
    }
}