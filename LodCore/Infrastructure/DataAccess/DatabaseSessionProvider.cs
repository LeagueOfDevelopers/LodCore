using DataAccess.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace DataAccess
{
    public class DatabaseSessionProvider
    {
        private readonly ISessionFactory _factory;

        public DatabaseSessionProvider()
        {
            var configuration = new Configuration();
            configuration.Configure();
            var modelMapper = new ModelMapper();
            modelMapper.AddMapping<ProfileMap>();
            modelMapper.AddMapping<UserMap>();
            modelMapper.AddMapping<ProjectMap>();
            modelMapper.AddMapping<EventMap>();
            modelMapper.AddMapping<DeliveryMap>();
            modelMapper.AddMapping<MailValidationRequestMap>();
            modelMapper.AddMapping<ProjectMembershipMap>();
            modelMapper.AddMapping<OrderMap>();
            configuration.AddDeserializedMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities(), null);

            _factory = configuration.BuildSessionFactory();

            new SchemaUpdate(configuration).Execute(false, true);
        }

        public ISession OpenSession()
        {
            return _factory.OpenSession();
        }
    }
}