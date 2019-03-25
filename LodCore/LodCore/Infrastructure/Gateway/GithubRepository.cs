using Journalist;

namespace LodCore.Infrastructure.Gateway
{
    public class GithubRepository
    {
        public GithubRepository(string name, string htmlUrl)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(htmlUrl, nameof(htmlUrl));

            Name = name;
            HtmlUrl = htmlUrl;
        }

        public string Name { get; }

        public string HtmlUrl { get; }
    }
}