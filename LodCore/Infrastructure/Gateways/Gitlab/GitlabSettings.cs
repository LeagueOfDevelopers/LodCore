using Journalist;

namespace Gateways.Gitlab
{
    public class GitlabSettings
    {
        public GitlabSettings(string host, string apiKey)
        {
            Require.NotEmpty(host, nameof(host));
            Require.NotEmpty(apiKey, nameof(apiKey));

            Host = host;
            ApiKey = apiKey;
        }

        public string Host { get; private set; }
        
        public string ApiKey { get; private set; } 
    }
}