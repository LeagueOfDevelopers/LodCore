using ProjectManagement;

namespace Gateways
{
    public interface IProjectManagerGateway
    {
        void AddNewUserToProject(int userId, int projectId);

        void RemoveUserFromProject(int userId, int projectId);

        void CreateProject(Project project);

        Project GetProject(int projectId);
    }
}
