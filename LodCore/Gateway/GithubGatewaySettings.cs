using Journalist;

namespace Gateway
{
    public class GithubGatewaySettings
    {
        public GithubGatewaySettings(
            string clientId,
            string clientSecret,
            string githubApiDefaultCallbackUri)
        {
            Require.NotEmpty(clientId, nameof(clientId));
            Require.NotEmpty(clientSecret, nameof(clientSecret));
            Require.NotEmpty(githubApiDefaultCallbackUri, nameof(githubApiDefaultCallbackUri));

            ClientId = clientId;
            ClientSecret = clientSecret;
            GithubApiDefaultCallbackUri = githubApiDefaultCallbackUri;
        }

        public readonly string ClientId;
        public readonly string ClientSecret;
        public readonly string GithubApiDefaultCallbackUri;
    }
}
