using System;
using System.Collections.Generic;
using System.Linq;
using NotificationService;
using OrderManagement.Application;
using OrderManagement.Domain.Events;
using OrderManagement.Infrastructure;

namespace OrderManagement.Domain
{
    internal class OrderManager : IOrderManager
    {
        private readonly IEventSink _orderManagmentEventSink;

        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository, IEventSink orderManagmentEventSink)
        {
            _orderRepository = orderRepository;
            _orderManagmentEventSink = orderManagmentEventSink;
        }

        public void AddOrder(Order newOrder)
        {
            _orderRepository.SaveProject(newOrder);

            _orderManagmentEventSink.ConsumeEvent(new OrderPlaced(newOrder.Id, newOrder.Header, newOrder.Description));
        }

        public Order GetOrder(int idOfOrder)
        {
            return _orderRepository.GetProject(idOfOrder);
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllProjects();
        }

        public List<Order> FindOrders(Func<Order, bool> criteria)
        {
            return _orderRepository.GetAllProjects().Where(criteria).ToList();
        }
    }
}