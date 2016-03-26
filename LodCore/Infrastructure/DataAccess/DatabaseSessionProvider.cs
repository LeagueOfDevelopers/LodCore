using System;
using System.Security.Policy;
using System.Threading;
using System.Web;
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
            modelMapper.AddMapping<UserMap>();
            modelMapper.AddMapping<ProjectMap>();
            modelMapper.AddMapping<EventMap>();
            modelMapper.AddMapping<DeliveryMap>();
            modelMapper.AddMapping<MailValidationRequestMap>();
            modelMapper.AddMapping<ProjectMembershipMap>();
            modelMapper.AddMapping<NotificationSettingMap>();
            modelMapper.AddMapping<OrderMap>();
            modelMapper.AddMapping<NotificationEmailMapping>();
            configuration.AddDeserializedMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities(), null);

            _factory = configuration.BuildSessionFactory();

            new SchemaUpdate(configuration).Execute(false, true);
        }

        public ISession GetCurrentSession()
        {
            return Session;
        }

        public void OpenSession()
        {
            if (Session == null || !Session.IsOpen)
            {
                Session = _factory.OpenSession();
            }
            
            if (!Transaction?.IsActive ?? true)
            {
                Transaction = Session.BeginTransaction();
            }
        }

        public void CloseSession()
        {
            if (Transaction != null && Transaction.IsActive)
            {
                Transaction.Commit();    
            }

            Session?.Dispose();
        }

        public void DropSession()
        {
            if (Transaction != null && Transaction.IsActive)
            {
                Transaction.Rollback();
                Transaction.Dispose();
            }

            Session?.Dispose();
        }

        public void ProcessInNHibernateSession(Action action)
        {
            OpenSession();
            action();
            CloseSession();
        }

        private const string SessionKey = "NHibernateSession";
        private const string TransactionKey = "NHibernateTransaction";

        private ISession Session
        {
            get
            {
                var session = HttpContext.Current?.Items[SessionKey] as ISession;
                return session 
                    ?? Thread.GetData(Thread.GetNamedDataSlot(SessionKey)) as ISession;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[SessionKey] = value;
                }
                else
                {
                    Thread.SetData(Thread.GetNamedDataSlot(SessionKey), value);
                }
            }
        }

        private ITransaction Transaction
        {
            get
            {
                var transaction = HttpContext.Current?.Items[TransactionKey] as ITransaction;
                return transaction
                   ?? Thread.GetData(Thread.GetNamedDataSlot(TransactionKey)) as ITransaction;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[TransactionKey] = value;
                }
                else
                {
                    Thread.SetData(Thread.GetNamedDataSlot(TransactionKey), value);
                }
            }
        }
    }
}