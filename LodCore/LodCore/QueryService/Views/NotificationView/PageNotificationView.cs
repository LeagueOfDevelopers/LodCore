using System;
using System.Collections.Generic;

namespace LodCore.QueryService.Views.NotificationView
{
    public class PageNotificationView
    {
        public PageNotificationView(IEnumerable<NotificationView> notifications)
        {
            Notifications = notifications ?? throw new ArgumentNullException(nameof(NotificationView));
        }

        public IEnumerable<NotificationView> Notifications { get; }
    }
}