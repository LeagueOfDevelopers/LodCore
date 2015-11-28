using Journalist;

namespace DataAccess.Entities
{
    public class Delivery
    {
        public Delivery(int userId, int eventId)
        {
            Require.Positive(userId, nameof(userId));
            Require.Positive(eventId, nameof(eventId));

            UserId = userId;
            EventId = eventId;
        }

        protected Delivery()
        {
        }

        public virtual int DeliveryId { get; protected set; }

        public virtual int UserId { get; protected set; }

        public virtual int EventId { get; protected set; }

        public virtual bool WasRead { get; set; } = false;
    }
}