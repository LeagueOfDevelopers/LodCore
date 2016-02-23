using System.Web.Http;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using OrderManagement.Application;

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
        public IHttpActionResult RegisterNewOrder([FromBody] RegisterNewOrderRequest request)
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
    }
}