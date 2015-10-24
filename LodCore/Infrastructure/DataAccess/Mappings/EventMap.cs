using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NotificationService;

namespace DataAccess.Mappings
{
    public class EventMap : ClassMapping<Event>
    {
        public EventMap()
        {
            Id(@event => @event.Id, mapper => mapper.Generator(Generators.Identity));
            Property(@event => @event.EventInfo, mapper => mapper.Column("EventInfo"));
            Property(@event => @event.EventType, mapper => mapper.Column("EventType"));
            Property(@event => @event.OccuredOn, mapper => mapper.Column("OccuredOn"));
        }
    }
}