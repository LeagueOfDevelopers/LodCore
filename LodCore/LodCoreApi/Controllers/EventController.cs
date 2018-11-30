using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LodCoreApi.Mappers;
using LodCoreApi.Models;
using Journalist;
using LodCore.Domain.NotificationService;
using LodCoreApi.Pagination;
using LodCoreApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using LodCore.QueryService.Handlers;
using LodCore.QueryService.Queries.NotificationQuery;
using LodCore.QueryService.Views.NotificationView;

namespace LodCoreApi.Controllers
{
    [Produces("application/json")]
    public class EventController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly EventMapper _eventMapper;
        private readonly IPaginationWrapper<Delivery> _paginationWrapper;
        private readonly INotificationHandler _notificationHandler;


        public EventController(INotificationService notificationService,
                               EventMapper eventMapper,
                               IPaginationWrapper<Delivery> paginationWrapper,
                               NotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
            _notificationService = notificationService;
            _eventMapper = eventMapper;
            _paginationWrapper = paginationWrapper;
        }

        [HttpPut]
        [Route("event/read")]
        //[Authorization(AccountRole.User)]
        [Authorize]
        public IActionResult MarkEventsAsRead([FromBody] int[] eventIds)
        {
            var userId = Request.GetUserId();
            _notificationService.MarkEventsAsRead(userId, eventIds);
            return Ok();
        }

        [HttpGet]
        [Route("event/{pageId}")]
        //[Authorization(AccountRole.User)]
        [Authorize]
        public IActionResult GetEventsByPage(int pageId,
            [FromQuery(Name = "offset")] int pageSize)
        {
            Require.ZeroOrGreater(pageId, nameof(pageId));
            var userId = Request.GetUserId();

            return Ok(_notificationHandler.Handle(
                new PageNotificationForDeveloperQuery(userId, pageId * pageSize, pageSize)));
        }
    }
}