using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace DataAccess
{
    public class DatabaseSessionProvider
    {
        public DatabaseSessionProvider(ModelMapper entityMapper)
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddDeserializedMapping(entityMapper.CompileMappingForAllExplicitlyAddedEntities(), null);

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