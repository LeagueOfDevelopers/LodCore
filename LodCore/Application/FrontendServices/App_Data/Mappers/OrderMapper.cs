using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FrontendServices.Models;
using Journalist;
using OrderManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class OrderMapper
    {
        public Order ToDomainEntity(RegisterNewOrderRequest request)
        {
            Require.NotNull(request, nameof(request));

            var order = new Order(
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
    }
}