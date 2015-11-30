using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IMailer _mailer;

        public OrderManagment(IOrderRepository orderRepository, IEventSink orderManagmentEventSink, IMailer mailer)
        {
            _orderRepository = orderRepository;
            _orderManagmentEventSink = orderManagmentEventSink;
            _mailer = mailer;
        }

        public void AddOrder(Order newOrder)
        {
            _orderRepository.SaveOrder(newOrder);

            _orderManagmentEventSink.ConsumeEvent(new OrderPlaced(newOrder.Id, newOrder.Header, newOrder.Description));

            _mailer.SendNewOrderEmail(newOrder);
        }

        public Order GetOrder(int idOfOrder)
        {
            return _orderRepository.GetOrder(idOfOrder);
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public List<Order> FindOrders(Func<Order, bool> criteria)
        {
            return _orderRepository.GetAllOrders().Where(criteria).ToList();
        }
    }
}