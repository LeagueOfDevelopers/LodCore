using System;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public interface IAuthorizer
    {
        TimeSpan TokenLifeTime { get; }
        bool CheckAuthorized(string authorizationToken, int userId);

        AuthorizationToken Authorize(string email, string password);
    }
}