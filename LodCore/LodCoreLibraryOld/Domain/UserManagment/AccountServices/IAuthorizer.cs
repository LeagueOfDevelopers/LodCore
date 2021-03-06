﻿using System;
using LodCoreLibraryOld.Common;

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