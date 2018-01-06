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

            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public readonly string ClientId;
        public readonly string ClientSecret;
    }
}
