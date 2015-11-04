using System;

namespace UserManagement.Application
{
    public interface IAuthorizer
    {
        bool CheckAuthorized(string authorizationToken);

        string Authorize(string email, string password);

        TimeSpan TokenLifeTime { get; }
    }
}