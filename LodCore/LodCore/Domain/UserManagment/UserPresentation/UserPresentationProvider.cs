using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using LodCore.Infrastructure.DataAccess.Repositories;

namespace LodCore.Domain.UserManagement
{
    public class UserPresentationProvider : IUserPresentationProvider
    {
        public UserPresentationProvider(INotificationSettingsRepository notificationSettingsRepository)
        {
            Require.NotNull(notificationSettingsRepository, nameof(notificationSettingsRepository));

            _notificationSettingsRepository = notificationSettingsRepository;
        }

        public NotificationSettingValue GetUserEventSettings(int userId, string eventType)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(eventType, nameof(eventType));

            var notificationType = NotificationSettings[eventType];

            var notificationSetting = _notificationSettingsRepository.ReadNotificationSettingByCriteria(setting =>
                setting.UserId == userId && setting.NotificationType == notificationType);

            return notificationSetting?.Value ?? DefaultNotificationSettingValue;
        }

        public void UpdateNotificationSetting(NotificationSetting notificationSetting)
        {
            Require.NotNull(notificationSetting, nameof(notificationSetting));

            var oldNotificationSetting = _notificationSettingsRepository.ReadNotificationSettingByCriteria(
                setting => setting.UserId == notificationSetting.UserId
                           && setting.NotificationType == notificationSetting.NotificationType);
            if (oldNotificationSetting != null)
            {
                oldNotificationSetting.Value = notificationSetting.Value;
                _notificationSettingsRepository.UpdateNotificationSetting(oldNotificationSetting);
            }
            else
            {
                _notificationSettingsRepository.CreateNotificationSetting(notificationSetting);
            }
        }

        private static readonly Dictionary<string, NotificationType> NotificationSettings =
            Enum.GetValues(typeof(NotificationType)).Cast<NotificationType>().ToDictionary(type => type.ToString());

        private readonly INotificationSettingsRepository _notificationSettingsRepository;

        private const NotificationSettingValue DefaultNotificationSettingValue = NotificationSettingValue.SendNotificationAndMail;
    }
}