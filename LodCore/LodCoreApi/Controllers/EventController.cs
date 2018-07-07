using System.Linq;
using System.Web.Http;
using LodCoreApi.App_Data;
using LodCoreApi.App_Data.Mappers;
using LodCoreApi.Authorization;
using LodCoreApi.Models;
using Journalist;
using LodCoreLibrary.Domain.NotificationService;
using LodCoreLibrary.Domain.UserManagement;

namespace LodCoreApi.Controllers
{
    public class EventController : ApiController
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
        [Authorization(AccountRole.User)]
        public IHttpActionResult MarkEventsAsRead([FromBody] int[] eventIds)
        {
            var userId = User.Identity.GetId();
            _notificationService.MarkEventsAsRead(userId, eventIds);
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
    }
}