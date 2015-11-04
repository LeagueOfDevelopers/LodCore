using System;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;

namespace Gateways
{
    public class ProjectManagerGateway : IProjectManagerGateway
    {
        public void AddNewUserToProject(Project project, int userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromProject(Project project, int userId)
        {
            throw new NotImplementedException();
        }

        public Issue[] GetProjectIssues(int projectId)
        {
            throw new NotImplementedException();
        }

        public int CreateProject(CreateProjectRequest request)
        {
            throw new NotImplementedException();
        }
    }
}