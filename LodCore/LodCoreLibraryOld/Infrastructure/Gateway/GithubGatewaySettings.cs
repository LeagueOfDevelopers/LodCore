using Journalist;

namespace LodCoreLibraryOld.Infrastructure.Gateway
{
    public class GithubGatewaySettings
    {
        public readonly string ClientId;
        public readonly string ClientSecret;
        public readonly string GithubApiDefaultCallbackUri;
        public readonly string OrganizationName;

        public GithubGatewaySettings(
            string clientId,
            string clientSecret,
            string githubApiDefaultCallbackUri,
            string organizationName)
        {
            Require.NotEmpty(clientId, nameof(clientId));
            Require.NotEmpty(clientSecret, nameof(clientSecret));
            Require.NotEmpty(githubApiDefaultCallbackUri, nameof(githubApiDefaultCallbackUri));
            Require.NotEmpty(organizationName, nameof(organizationName));

            ClientId = clientId;
            ClientSecret = clientSecret;
            GithubApiDefaultCallbackUri = githubApiDefaultCallbackUri;
            OrganizationName = organizationName;
        }
    }
}