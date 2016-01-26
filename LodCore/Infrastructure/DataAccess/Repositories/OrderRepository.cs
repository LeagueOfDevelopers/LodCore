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

        public Order GetOrder(int orderId)
        {
            Require.Positive(orderId, nameof(orderId));

            var session = _databaseSessionProvider.GetCurrentSession();
            return session.Get<Order>(orderId);
        }

        public int SaveOrder(Order order)
        {
            Require.NotNull(order, nameof(order));

            var session = _databaseSessionProvider.GetCurrentSession();
            return (int) session.Save(order);
        }

        public List<Order> GetAllOrders(Func<Order, bool> criteria = null)
        {
            var session = _databaseSessionProvider.GetCurrentSession();
            var orders = criteria == null 
                ? session.Query<Order>() 
                : session.Query<Order>().Where(criteria);
            return orders.ToList();
        }
    }
}