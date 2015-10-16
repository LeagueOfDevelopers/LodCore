using System;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;

namespace Gateways
{
    public class VersionControlSystemGateway : IVersionControlSystemGateway
    {
        public Uri CreateRepositoryForProject(CreateProjectRequest request)
        {
            throw new NotImplementedException();
        }

        public void AddUserToProject(Project project, int userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromProject(Project project, int userId)
        {
            throw new NotImplementedException();
        }
    }
}