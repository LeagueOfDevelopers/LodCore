using System.Threading.Tasks;

namespace Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPage();

        Task<string> GetTokenByCode(string code);

        bool StateIsValid(string state);
    }
}
