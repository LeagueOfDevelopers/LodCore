namespace LodCoreLibraryOld.Domain.ProjectManagment
{
    public interface IUserRoleAnalyzer
    {
        string GetUserCommonRole(int userId);
    }
}