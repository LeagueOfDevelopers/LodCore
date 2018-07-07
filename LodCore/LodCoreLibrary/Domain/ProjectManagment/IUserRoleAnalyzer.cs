namespace LodCoreLibrary.Domain.ProjectManagment
{
    public interface IUserRoleAnalyzer
    {
        string GetUserCommonRole(int userId);
    }
}