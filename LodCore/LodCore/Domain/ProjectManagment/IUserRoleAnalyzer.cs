namespace LodCore.Domain.ProjectManagment
{
    public interface IUserRoleAnalyzer
    {
        string GetUserCommonRole(int userId);
    }
}