using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Authorization;
using Journalist;
using NotificationService;
using Event = FrontendServices.Models.Event;

namespace FrontendServices.Controllers
{
    public class EventController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly EventMapper eventMapper;

        public EventController(INotificationService notificationService, EventMapper eventMapper)
        {
            _notificationService = notificationService;
            this.eventMapper = eventMapper;
        }

        [HttpPut]
        [Route("event/read")]
        public IHttpActionResult MarkEventsAsRead([FromBody] int[] eventIds)
        {
            _notificationService.MarkEventsAsRead(eventIds);
            return Ok();
        }

        [HttpGet]
        [Route("event/{pageId}")]
        public IEnumerable<Event> GetEventsByPage(int pageId)
        {
            Require.ZeroOrGreater(pageId, nameof(pageId));

            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }
            var userId = User.Identity.GetId();

            var events = _notificationService.GetEventsForUser(userId, pageId).ToList();


            return events.Select(@event => eventMapper.ToEventPageEvent(@event, userId));
        }

        [HttpGet]
        [Route("event/count")]
        public int GetCountOfUnreadEvents()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }

            var userId = User.Identity.GetId();

            return _notificationService.GetNumberOfUnreadEvents(userId);
        }
    }
}