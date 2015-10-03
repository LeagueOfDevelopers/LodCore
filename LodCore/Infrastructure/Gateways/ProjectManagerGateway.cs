using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;

namespace Gateways
{
    public class ProjectManagerGateway : IProjectManagerGateway
    {
        public void AddNewUserToProject(int userId, int projectId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveUserFromProject(int userId, int projectId)
        {
            throw new System.NotImplementedException();
        }

        public void CreateProject(Project project)
        {
            throw new System.NotImplementedException();
        }

        public Project GetProject(int projectId)
        {
            throw new System.NotImplementedException();
        }
    }
}