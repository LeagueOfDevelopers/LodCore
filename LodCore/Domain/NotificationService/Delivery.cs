namespace NotificationService
{
    public class Delivery
    {
        public Delivery(int receiverId, int eventId)
        {
            ReceiverId = receiverId;
            EventId = eventId;
            IsReceived = false;
        }

        protected int DeliveryId { get; set; }

        public int ReceiverId { get; protected set; }

        public int EventId { get; protected set; }

        public bool IsReceived { get; set; }
    }
}