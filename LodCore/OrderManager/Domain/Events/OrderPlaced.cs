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
            ShortDescription = description.Length > 50 ? description.Substring(0, 50) : description;
        }

        public int UserId { get; private set; }
        public string Header { get; private set; }
        public string ShortDescription { get; private set; }
    }
}