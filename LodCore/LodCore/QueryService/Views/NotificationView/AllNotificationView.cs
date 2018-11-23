using System;
using System.Collections.Generic;
using System.Text;
using LodCore.QueryService.DTOs;
namespace LodCore.QueryService.Views.NotificationView
{
    public class AllNotificationView
    {
        public AllNotificationView(IEnumerable<NotificationDto> notifications)
        {
            Notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
        }

        public IEnumerable<NotificationDto> Notifications { get; private set; }

    }
}
