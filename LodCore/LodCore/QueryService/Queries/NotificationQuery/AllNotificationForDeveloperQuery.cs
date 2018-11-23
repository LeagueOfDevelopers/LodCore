using System;
using System.Collections.Generic;
using System.Text;
using LodCore.QueryService.Views.NotificationView;
using LodCore.QueryService.DTOs;

namespace LodCore.QueryService.Queries.NotificationQuery
{
    public class AllNotificationForDeveloperQuery : IQuery<AllNotificationView>
    {
        public AllNotificationForDeveloperQuery(int developerID)
        {
            DeveloperID = developerID;
            Sql = ""; // ToDo sql query
        }

        public string Sql { get; }
        public int DeveloperID { get; }

        public AllNotificationView FormResult(IEnumerable<NotificationDto> rawResult)
        {
            return new AllNotificationView(rawResult);
        }
    }
}
