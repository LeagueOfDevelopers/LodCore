using Journalist;

namespace Gateway
{
    public class GithubGatewaySettings
    {
        public GithubGatewaySettings(
            string clientId,
            string clientSecret)
        {
            Require.NotEmpty(clientId, nameof(clientId));
            Require.NotEmpty(clientSecret, nameof(clientSecret));

            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        private readonly string _clientId;
        private readonly string _clientSecret;
    }
}
