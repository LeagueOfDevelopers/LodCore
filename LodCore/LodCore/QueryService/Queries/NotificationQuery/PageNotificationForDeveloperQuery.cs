using LodCore.QueryService.Views.NotificationView;

namespace LodCore.QueryService.Queries.NotificationQuery
{
    public class PageNotificationForDeveloperQuery : IQuery<PageNotificationView>
    {
        public PageNotificationForDeveloperQuery(int developerID, int offset, int pageSize)
        {
            DeveloperID = developerID;
            Offset = offset;
            PageSize = pageSize;
            Sql = "SELECT WasRead, EventId, OccuredOn, EventType, EventInfo FROM delivery " +
                  "JOIN accounts ON accounts.UserId = delivery.OrderId " +
                  "JOIN eventinfo ON eventinfo.Id = delivery.EventId " +
                  "where accounts.UserId = @developerID " +
                  "LIMIT @pageSize OFFSET @offset ";
        }

        public string Sql { get; }
        public int DeveloperID { get; }
        public int Offset { get; }
        public int PageSize { get; }
    }
}