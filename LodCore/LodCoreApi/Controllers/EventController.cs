using LodCore.Domain.NotificationService;
using LodCore.QueryService.Handlers;
using LodCoreApi.Mappers;
using LodCoreApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace LodCoreApi.Controllers
{
    [Produces("application/json")]
    public class EventController : Controller
    {
        private readonly EventMapper _eventMapper;
        private readonly INotificationHandler _notificationHandler;
        private readonly INotificationService _notificationService;
        private readonly IPaginationWrapper<Delivery> _paginationWrapper;


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

        //[HttpPut]
        //[Route("event/read")]
        ////[Authorization(AccountRole.User)]
        //[Authorize]
        //public IActionResult MarkEventsAsRead([FromBody] int[] eventIds)
        //{
        //    var userId = Request.GetUserId();
        //    _notificationService.MarkEventsAsRead(userId, eventIds);
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("event/{pageId}")]
        ////[Authorization(AccountRole.User)]
        //[Authorize]
        //public IActionResult GetEventsByPage(int pageId)
        //{
        //    Require.ZeroOrGreater(pageId, nameof(pageId));
        //    var userId = Request.GetUserId();

        //    return Ok(_notificationHandler.Handle(
        //        new PageNotificationForDeveloperQuery(userId,
        //        pageId * _notificationHandler.PaginationSettings,
        //        _notificationHandler.PaginationSettings)));
        //}
    }
}