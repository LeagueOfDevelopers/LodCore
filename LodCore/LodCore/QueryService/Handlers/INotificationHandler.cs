using LodCore.QueryService.Queries.NotificationQuery;
using LodCore.QueryService.Views.NotificationView;

namespace LodCore.QueryService.Handlers
{
    public interface INotificationHandler :
        IQueryHandler<PageNotificationForDeveloperQuery, PageNotificationView>
    {
        int PaginationSettings { get; }
    }
}