using DataAccess.Mappings.Application;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using OrderManagement.Domain;

namespace DataAccess.Mappings
{
    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(order => order.Id, mapper =>
            {
                mapper.Column("Id");
                mapper.Generator(Generators.Identity);
            });
            Property(order => order.Header, mapper => mapper.Column("Header"));
            Property(order => order.CustomerName, mapper => mapper.Column("CustomerName"));
            Property(order => order.CreatedOnDateTime, mapper => mapper.Column("CreatedOnDateTime"));
            Property(order => order.DeadLine, mapper => mapper.Column("DeadLine"));
            Property(order => order.Email, mapper =>
            {
                mapper.Column("Email");
                mapper.Type<MailAddressType>();
            });
            Property(order => order.Description, mapper => mapper.Column("Description"));
            Set(order => order.Attachments, mapper =>
            {
                mapper.Table("OrderAttachments");
                mapper.Cascade(Cascade.All);
            });
            Property(order => order.ProjectType, mapper => mapper.Column("ProjectType"));
        }
    }
}