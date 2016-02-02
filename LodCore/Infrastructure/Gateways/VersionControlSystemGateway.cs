using System;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace Gateways
{
    public class VersionControlSystemGateway : IVersionControlSystemGateway, IGitlabUserRegistrar
    {
        public int CreateRepositoryForProject(CreateProjectRequest request)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRepository(Project project, int userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromProject(Project project, int userId)
        {
            throw new NotImplementedException();
        }

        public int RegisterUser(CreateAccountRequest request)
        {
            //todo: implement
            return 1;
        }
    }
}