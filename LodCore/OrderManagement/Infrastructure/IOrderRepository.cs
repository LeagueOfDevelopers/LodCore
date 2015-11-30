using System;
using System.Collections.Generic;
using OrderManagement.Domain;

namespace OrderManagement.Infrastructure
{
    public interface IOrderRepository
    {
        List<Order> GetAllProjects(Func<Order, bool> criteria = null);

        Order GetProject(int projectId);

        int SaveProject(Order project);
    }
}