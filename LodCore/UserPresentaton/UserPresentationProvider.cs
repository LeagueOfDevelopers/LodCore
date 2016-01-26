using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;

namespace UserPresentaton
{
    public class UserPresentationProvider : IUserPresentationProvider
    {
        private static readonly Dictionary<string, NotificationType> NotificationSettings =
            Enum.GetValues(typeof (NotificationType)).Cast<NotificationType>().ToDictionary(type => type.ToString());

        private readonly INotificationSettingsRepository _notificationSettingsRepository;

        public UserPresentationProvider(INotificationSettingsRepository notificationSettingsRepository)
        {
            Require.NotNull(notificationSettingsRepository, nameof(notificationSettingsRepository));

            _notificationSettingsRepository = notificationSettingsRepository;
        }

        public NotificationSetting GetUserEventSettings(int userId, string eventType)
        {
            var notificationType = NotificationSettings[eventType];

            return _notificationSettingsRepository.ReadNotificationSettingByCriteria(setting =>
                setting.UserId == userId && setting.NotificationType == notificationType);
        }
    }
}