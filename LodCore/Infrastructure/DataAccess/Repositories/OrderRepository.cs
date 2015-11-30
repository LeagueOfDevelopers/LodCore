using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using NHibernate.Linq;
using OrderManagement.Domain;
using OrderManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseSessionProvider _databaseSessionProvider;

        public OrderRepository(DatabaseSessionProvider databaseSessionProvider)
        {
            Require.NotNull(databaseSessionProvider, nameof(databaseSessionProvider));

            _databaseSessionProvider = databaseSessionProvider;
        }

        public List<Order> GetAllProjects(Func<Order, bool> criteria = null)
        {
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Query<Order>().ToList();
            }
        }

        public Order GetProject(int projectId)
        {
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Get<Order>(projectId);
            }
        }

        public int SaveProject(Order project)
        {
            Require.NotNull(project, nameof(project));
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return (int) session.Save(project);
            }
        }
    }
}