using LodCoreLibraryOld.Common;
using System;

namespace LodCoreLibraryOld.Domain.UserManagement
{
    public interface IAuthorizer
    {
        TimeSpan TokenLifeTime { get; }

        AuthorizationTokenInfo GetTokenInfo(string authorizationToken);

        AuthorizationTokenInfo Authorize(string email, Password password);

        AuthorizationTokenInfo AuthorizeWithGithub(string linkToGithubProfile);
    }
}