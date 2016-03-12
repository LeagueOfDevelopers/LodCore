using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FrontendServices.Authorization;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain;

namespace FrontendServices.Controllers
{
    public class EventController : ApiController
    {
        private readonly INotificationService _notificationService;

        public EventController(INotificationService notificationService)
        {
            _notificationService = notificationService;
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

            return _notificationService.GetEventsForUser(userId, pageId).ToList();
        }
    }
}