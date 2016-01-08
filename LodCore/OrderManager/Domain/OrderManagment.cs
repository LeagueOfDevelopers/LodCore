using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using NotificationService;
using OrderManagement.Application;
using OrderManagement.Domain.Events;
using OrderManagement.Infrastructure;

namespace OrderManagement.Domain
{
    public class OrderManagment : IOrderManager
    {
        private readonly IEventSink _orderManagmentEventSink;

        private readonly IOrderRepository _orderRepository;

        public OrderManagment(IOrderRepository orderRepository, IEventSink orderManagmentEventSink)
        {
            Require.NotNull(orderRepository, nameof(orderRepository));
            Require.NotNull(orderManagmentEventSink, nameof(orderManagmentEventSink));

            _orderRepository = orderRepository;
            _orderManagmentEventSink = orderManagmentEventSink;
        }

        public void AddOrder(Order newOrder)
        {
            Require.NotNull(newOrder, nameof(newOrder));

            _orderRepository.SaveOrder(newOrder);

            _orderManagmentEventSink.ConsumeEvent(new OrderPlaced(newOrder.Id));
        }

        public Order GetOrder(int idOfOrder)
        {
            Require.Positive(idOfOrder, nameof(idOfOrder));
            var order = _orderRepository.GetOrder(idOfOrder);
            if (order == null)
            {
                throw new OrderNotFoundException();
            }
            return order;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public List<Order> FindOrders(Func<Order, bool> criteria)
        {
            Require.NotNull(criteria, nameof(criteria));
            var orders = _orderRepository.GetAllOrders(criteria);
            return orders;
        }
    }
}