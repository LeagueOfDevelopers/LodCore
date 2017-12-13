using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FrontendServices.App_Data;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Authorization;
using FrontendServices.Models;
using Journalist;
using NotificationService;
using UserManagement.Domain;
using Event = FrontendServices.Models.Event;

namespace FrontendServices.Controllers
{
    public class EventController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly EventMapper _eventMapper;
        private readonly IPaginationWrapper<Delivery> _paginationWrapper; 

        public EventController(INotificationService notificationService, EventMapper eventMapper, IPaginationWrapper<NotificationService.Delivery> paginationWrapper)
        {
            _notificationService = notificationService;
            _eventMapper = eventMapper;
            _paginationWrapper = paginationWrapper;
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
        [Authorization(AccountRole.User)]
        public PaginableObject GetEventsByPage(int pageId)
        {
            Require.ZeroOrGreater(pageId, nameof(pageId));

            var userId = User.Identity.GetId();

            var events = _notificationService.GetEventsForUser(userId, pageId).ToList();

            var eventsPreview = events.Select(@event => _eventMapper.ToEventPageEvent(@event, userId));
            return _paginationWrapper.WrapResponse(eventsPreview, @event => @event.UserId == userId);
        }

        [HttpGet]
        [Route("event/count")]
        [Authorization(AccountRole.User)]
        public int GetCountOfUnreadEvents()
        {
            var userId = User.Identity.GetId();

            return _notificationService.GetNumberOfUnreadEvents(userId);
        }
    }
}