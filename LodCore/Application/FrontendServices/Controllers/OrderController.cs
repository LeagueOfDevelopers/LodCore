using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Authorization;
using Journalist;
using OrderManagement.Application;
using OrderManagement.Domain;
using UserManagement.Domain;
using Order = FrontendServices.Models.Order;

namespace FrontendServices.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderManager _orderManager;
        private readonly OrderMapper _orderMapper;

        public OrderController(IOrderManager orderManager, OrderMapper orderMapper)
        {
            Require.NotNull(orderManager, nameof(orderManager));
            Require.NotNull(orderMapper, nameof(orderMapper));

            _orderManager = orderManager;
            _orderMapper = orderMapper;
        }

        [HttpPost]
        [Route("orders")]
        public IHttpActionResult RegisterNewOrder([FromBody] Order request)
        {
            Require.NotNull(request, nameof(request));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = _orderMapper.ToDomainEntity(request);
            _orderManager.AddOrder(order);
            return Ok();
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("orders")]
        public IEnumerable<Order> GetAllOrders()
        {
            var orders = _orderManager.GetAllOrders();
            return orders.Select(order => _orderMapper.ToModel(order));
        }

        [HttpGet]
        [Authorization(AccountRole.Administrator)]
        [Route("orders/{id}")]
        public IHttpActionResult GetOrder(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var order = _orderManager.GetOrder(id);
                return Ok(_orderMapper.ToModel(order));
            }
            catch (OrderNotFoundException)
            {
                return NotFound();
            }
        } 
    }
}