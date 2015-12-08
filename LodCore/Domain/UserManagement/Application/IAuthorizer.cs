using System;
using Common;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public interface IAuthorizer
    {
        bool CheckAuthorized(string authorizationToken, int userId);

        AuthorizationToken Authorize(string email, Password password);

        TimeSpan TokenLifeTime { get; }
    }
}