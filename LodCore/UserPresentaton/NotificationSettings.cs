namespace UserPresentaton
{
    public class NotificationSettings
    {
        public virtual int SettingId { get; protected set; }

        public virtual int UserId { get; protected set; }

        public virtual string NotificationType { get; protected set; }

        public virtual NotificationSettingEnum Value { get; protected set; }

        protected NotificationSettings()
        {
            
        }

        public NotificationSettings(int userId, string notificationType, NotificationSettingEnum value)
        {
            UserId = userId;
            NotificationType = notificationType;
            Value = value;
        }
    }
}