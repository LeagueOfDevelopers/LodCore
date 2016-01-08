using System;
using System.Collections.Generic;
using OrderManagement.Domain;

namespace OrderManagement.Infrastructure
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders(Func<Order, bool> criteria = null);

        Order GetOrder(int orderId);

        int SaveOrder(Order order);
    }
}