namespace DataAccess.Entities
{
    public class EventDto
    {
        public EventDto(int id, string eventType, byte[] eventBody)
        {
            Id = id;
            EventType = eventType;
            EventBody = eventBody;
        }

        public int Id { get; protected set; }
        
        public string EventType { get; protected set; }

        public byte[] EventBody { get; protected set; }
    }
}