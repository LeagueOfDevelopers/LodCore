using System.Threading.Tasks;

namespace Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPage();

        Task<string> GetTokenByCode(string code);

        Task<string> GetLinkToUserGithubProfile(string token);

        bool StateIsValid(string state);
    }
}
