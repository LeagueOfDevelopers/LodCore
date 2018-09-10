using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LodCore.Mappers;
using LodCore.Authorization;
using LodCore.Models;
using Journalist;
using LodCoreLibrary.Domain.NotificationService;
using LodCore.Pagination;

namespace LodCore.Controllers
{
    [Produces("application/json")]
    public class EventController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly EventMapper _eventMapper;
        private readonly IPaginationWrapper<Delivery> _paginationWrapper;

        public EventController(INotificationService notificationService,
                               EventMapper eventMapper,
                               IPaginationWrapper<Delivery> paginationWrapper)
        {
            _notificationService = notificationService;
            _eventMapper = eventMapper;
            _paginationWrapper = paginationWrapper;
        }

        [HttpPut]
        [Route("event/read")]
        //[Authorization(AccountRole.User)]
        public IActionResult MarkEventsAsRead([FromBody] int[] eventIds)
        {
            var userId = User.Identity.GetId();
            _notificationService.MarkEventsAsRead(userId, eventIds);
            return Ok();
        }

        [HttpGet]
        [Route("event/{pageId}")]
        //[Authorization(AccountRole.User)]
        public PaginableObject GetEventsByPage(int pageId)
        {
            Require.ZeroOrGreater(pageId, nameof(pageId));

            var userId = User.Identity.GetId();

            var events = _notificationService.GetEventsForUser(userId, pageId).ToList();

            var eventsPreview = events.Select(@event => _eventMapper.ToEventPageEvent(@event, userId));
            return _paginationWrapper.WrapResponse(eventsPreview, @event => @event.UserId == userId);
        }
    }
}