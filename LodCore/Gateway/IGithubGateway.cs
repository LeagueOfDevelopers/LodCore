namespace Gateway
{
    public interface IGithubGateway
    {
        string GetLinkToGithubLoginPage(int currentUserId);

        string GetTokenByCode(string code);

        string GetLinkToUserGithubProfile(string token);

        bool StateIsValid(string state);
    }
}
