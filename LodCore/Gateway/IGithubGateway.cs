namespace Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPageToSignUp(int registeredUserId);

        string GetLinkToGithubLoginPage(int currentUserId);

        string GetLinkToGithubLoginPage();

        string GetTokenByCode(string code);

        string GetLinkToUserGithubProfile(string token);

        bool StateIsValid(string state);
    }
}
