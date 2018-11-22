using Journalist;

namespace LodCoreLibraryOld.Infrastructure.Gateway
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

        public string Name { get; private set; }

        public string HtmlUrl { get; private set; }
    }
}
