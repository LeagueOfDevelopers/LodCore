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

        public List<Order> GetAllOrders()
        {
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Query<Order>().ToList();
            }
        }

        public Order GetOrder(int orderId)
        {
            Require.Positive(orderId, nameof(orderId));
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Get<Order>(orderId);
            }
        }

        public int SaveOrder(Order order)
        {
            Require.NotNull(order, nameof(order));
            Require.NotNull(order, nameof(order));
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return (int) session.Save(order);
            }
        }

        public List<Order> FindOrdersByCriteria(Func<Order, bool> criteria = null)
        {
            Require.NotNull(criteria, nameof(criteria));
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Query<Order>().ToList().FindAll(order => criteria(order));
            }
        }

        public List<Order> GetAlFlOrders(Func<Order, bool> criteria = null)
        {
            Require.NotNull(criteria, nameof(criteria));
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Query<Order>().ToList();
            }
        }
    }
}