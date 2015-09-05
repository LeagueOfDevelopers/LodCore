namespace AccountService
{
    public interface IAuthorizationService
    {
        void Authorize(AccountCredentials credentials);

        bool CheckAuthorized(string authorizationToken);
    }
}