using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FrontendServices.Models;
using Journalist;
using OrderManagement.Domain;
using Order = FrontendServices.Models.Order;

namespace FrontendServices.App_Data.Mappers
{
    public class OrderMapper
    {
        public OrderManagement.Domain.Order ToDomainEntity(Order request)
        {
            Require.NotNull(request, nameof(request));

            var order = new OrderManagement.Domain.Order(
                request.Header,
                request.CustomerName,
                DateTime.Now,
                request.DeadLine,
                new MailAddress(request.Email),
                request.Description,
                new HashSet<Uri>(request.Attachments.Select(attachment => new Uri(attachment))),
                request.ProjectType);

            return order;
        }

        public Order ToModel(OrderManagement.Domain.Order order)
        {
            Require.NotNull(order, nameof(order));

            var orderModel = new Order()
            {
                ProjectType = order.ProjectType,
                Email = order.Email.Address,
                CustomerName = order.CustomerName,
                DeadLine = order.DeadLine,
                Description = order.Description,
                Attachments = order.Attachments.Select(uri => uri.AbsoluteUri).ToArray(),
                Header = order.Header
            };

            return orderModel;
        }
    }
}