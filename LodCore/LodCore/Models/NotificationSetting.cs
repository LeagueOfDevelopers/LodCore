using LodCoreLibrary.Domain.UserManagement;
using System.ComponentModel.DataAnnotations;

namespace LodCore.Models
{
    public class NotificationSetting
    {
        [EnumDataType(typeof (NotificationType), ErrorMessage = "Unknown notification type")]
        public NotificationType NotificationType { get; set; }

        [EnumDataType(typeof (NotificationSettingValue), ErrorMessage = "Unknown notification setting value")]
        public NotificationSettingValue NotificationSettingValue { get; set; }
    }
}