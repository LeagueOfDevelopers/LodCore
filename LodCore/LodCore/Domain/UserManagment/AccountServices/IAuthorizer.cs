using LodCore.Common;
using System;

namespace LodCore.Domain.UserManagement
{
    public interface IAuthorizer
    {
        TimeSpan TokenLifeTime { get; }

        AuthorizationTokenInfo GetTokenInfo(string authorizationToken);

        AuthorizationTokenInfo Authorize(string email, Password password);

        AuthorizationTokenInfo AuthorizeWithGithub(string linkToGithubProfile);
    }
}