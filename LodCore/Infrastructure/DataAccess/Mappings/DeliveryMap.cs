using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NotificationService;

namespace DataAccess.Mappings
{
    public class DeliveryMap : ClassMapping<Delivery>
    {
        public DeliveryMap()
        {
            Id(model => model.DeliveryId, mapper => mapper.Generator(Generators.Identity));
            Property(model => model.UserId, mapper => mapper.Column("OrderId"));
            Property(model => model.EventId, mapper => mapper.Column("EventId"));
            Property(model => model.WasRead, mapper => mapper.Column("WasRead"));
        }
    }
}