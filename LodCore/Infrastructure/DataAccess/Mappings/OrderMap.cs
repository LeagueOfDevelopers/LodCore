using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode.Conformist;
using OrderManagement.Domain;

namespace DataAccess.Mappings
{
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(order => order.Id, mapper => mapper.Column("Id"));
            Property(order => order.Header, mapper => mapper.Column("Header"));
            Property(order => order.CreatedOnDateTime, mapper => mapper.Column("CreatedOnDateTime"));
            Property(order => order.Email, mapper =>
            {
                mapper.Column("Email");
                mapper.Type<MailAddressType>();
            });
            Property(order => order.Description, mapper => mapper.Column("Description"));
            Property(order => order.Attachment, mapper => mapper.Column("Attachment"));
            Property(order => order.ProjectType, mapper => mapper.Column("ProjectType"));
        }
    }
}