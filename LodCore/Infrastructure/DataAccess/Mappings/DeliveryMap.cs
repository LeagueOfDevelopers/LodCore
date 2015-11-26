using DataAccess.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DataAccess.Mappings
{
    public class DeliveryMap : ClassMapping<Delivery>
    {
        public DeliveryMap()
        {
            Property(model => model.UserId, mapper => mapper.Column("UserId"));
            Property(model => model.EventId, mapper => mapper.Column("EventId"));
        }
    }
}