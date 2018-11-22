using Journalist;

namespace LodCoreLibraryOld.Infrastructure.EventBus
{
    public class EventBusSettings
    {
        public EventBusSettings(
            string hostName,
            string virtualHost,
            string userName,
            string password)
        {
            Require.NotEmpty(hostName, nameof(hostName));
            Require.NotEmpty(virtualHost, nameof(virtualHost));
            Require.NotEmpty(userName, nameof(userName));
            Require.NotEmpty(password, nameof(password));

            HostName = hostName;
            VirtualHost = virtualHost;
            UserName = userName;
            Password = password;
        }

        public string HostName;

        public string VirtualHost;

        public string UserName;

        public string Password;
    }
}
