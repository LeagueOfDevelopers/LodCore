using System;
using System.Collections.Generic;
using OrderManagement.Domain;

namespace OrderManagement.Infrastructure
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();

        Order GetOrder(int orderId);

        int SaveOrder(Order order);

        List<Order> FindOrdersByCriteria(Func<Order, bool> criteria = null);
    }
}