﻿using System.Net.Mail;

namespace NotificationService
{
    public interface IMailer
    {
        void SendNotificationEmail(int[] userIds, IEventInfo eventInfo);
    }
}