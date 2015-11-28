using System;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public interface IAuthorizer
    {
        bool CheckAuthorized(string authorizationToken, int userId);

        AuthorizationToken Authorize(string email, string password);

        TimeSpan TokenLifeTime { get; }
    }
}