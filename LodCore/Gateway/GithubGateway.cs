using System;
using Journalist;
using Octokit;

namespace Gateway
{
    public class GithubGateway : IGithubGateway
    {
        public GithubGateway(GithubGatewaySettings githubGatewaySettings)
        {
            Require.NotNull(githubGatewaySettings, nameof(githubGatewaySettings));

            _githubGatewaySettings = githubGatewaySettings;
        }

        private readonly GithubGatewaySettings _githubGatewaySettings;
    }
}
