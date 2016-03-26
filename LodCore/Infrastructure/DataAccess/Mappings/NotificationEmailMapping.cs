using Mailing.AsyncMailing;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DataAccess.Mappings
{
    public class NotificationEmailMapping : ClassMapping<NotificationEmail>
    {
        public NotificationEmailMapping()
        {
            Id(model => model.Id, mapper => mapper.Generator(Generators.Native));
            Lazy(false);
            Set(model => model.UserIds, mapper =>
            {
                mapper.Table("EmailReceivers");
                mapper.Cascade(Cascade.All);
                mapper.Lazy(CollectionLazy.NoLazy);
            }, mod => mod.Element());
            Property(model => model.NotificationDescription);
        }
    }
}