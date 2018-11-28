using System;
using System.Collections.Generic;
using System.Text;
using LodCore.QueryService.Views.NotificationView;
using LodCore.QueryService.DTOs;

namespace LodCore.QueryService.Queries.NotificationQuery
{
    public class PageNotificationForDeveloperQuery : IQuery<PageNotificationView>
    {
        public PageNotificationForDeveloperQuery(int developerID, int offset, int pageSize)
        {
            DeveloperID = developerID;
            Sql = "SELECT WasRead, EventId, OccuredOn, EventType, EventInfo FROM test.delivery" +
                  "JOIN test.accounts ON test.accounts.UserId = test.delivery.OrderId" +
                  "JOIN test.eventinfo ON test.eventinfo.Id = test.delivery.EventId" +
                  $"where test.accounts.UserId = {developerID}"+
                  $"LIMIT {pageSize} OFFSET {offset}";
        }

        public string Sql { get; }
        public int DeveloperID { get; }
    }
}
