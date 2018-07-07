using LodCoreLibrary.Common;
using System;

namespace LodCoreLibrary.Domain.UserManagement
{
    public interface IAuthorizer
    {
        TimeSpan TokenLifeTime { get; }

        AuthorizationTokenInfo GetTokenInfo(string authorizationToken);

        AuthorizationTokenInfo Authorize(string email, Password password);

        AuthorizationTokenInfo AuthorizeWithGithub(string linkToGithubProfile);
    }
}