using Journalist;

namespace Gateways.Redmine
{
    public class RedmineSettings
    {
        public RedmineSettings(string redmineHost, string apiKey)
        {
            Require.NotEmpty(redmineHost, nameof(redmineHost));
            Require.NotEmpty(apiKey, nameof(apiKey));

            RedmineHost = redmineHost;
            ApiKey = apiKey;
        }

        public string RedmineHost { get; private set; }

        public string ApiKey { get; private set; }
    }
}