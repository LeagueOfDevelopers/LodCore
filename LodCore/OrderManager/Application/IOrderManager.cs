using System;
using System.Collections.Generic;
using OrderManagement.Domain;

namespace OrderManagement.Application
{
    public interface IOrderManager
    {
        void AddOrder(Order newOrder);

        Order GetOrder(int idOfOrder);

        List<Order> GetAllOrders();

        List<Order> FindOrders(Func<Order, bool> criteria);
    }
}