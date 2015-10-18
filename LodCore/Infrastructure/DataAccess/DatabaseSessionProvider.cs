using DataAccess.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace DataAccess
{
    public class DatabaseSessionProvider
    {
        public DatabaseSessionProvider()
        {
            var configuration = new Configuration();
            configuration.Configure();
            var modelMapper = new ModelMapper();
            modelMapper.AddMapping<ProfileMap>();
            modelMapper.AddMapping<UserMap>();
            modelMapper.AddMapping<ProjectMap>();
            configuration.AddDeserializedMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities(), null);

            _factory = configuration.BuildSessionFactory();

            new SchemaExport(configuration).Execute(false, true, false);
        }

        public ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        private readonly ISessionFactory _factory;
    }
}