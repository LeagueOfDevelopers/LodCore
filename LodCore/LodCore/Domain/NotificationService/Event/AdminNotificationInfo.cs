using Journalist;

namespace LodCore.Domain.NotificationService
{
    public class AdminNotificationInfo : EventInfoBase
    {
        public string InfoText;

        public AdminNotificationInfo(string infoText)
        {
            Require.NotEmpty(infoText, nameof(infoText));

            InfoText = infoText;
        }
    }
}