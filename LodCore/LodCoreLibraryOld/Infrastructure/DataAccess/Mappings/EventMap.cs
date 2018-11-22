using LodCoreLibraryOld.Domain.NotificationService;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Mappings
{
    public class EventMap : ClassMapping<Event>
    {
        public EventMap()
        {
            Table("eventinfo");
            Id(@event => @event.Id, mapper => mapper.Generator(Generators.Identity));
            Property(@event => @event.EventInfo, mapper =>
            {
                mapper.Column("EventInfo");
                mapper.Length(1000);
            });
            Property(@event => @event.EventType, mapper => mapper.Column("EventType"));
            Property(@event => @event.OccuredOn, mapper => mapper.Column("OccuredOn"));
        }
    }
}