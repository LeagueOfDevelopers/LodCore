using LodCoreLibrary.Domain.ProjectManagment;

namespace LodCoreApi.App_Data.Authorization
{
    public static class ProjectsPolicies
    {
        public static bool OnlyDoneOrInProgress(Project project)
        {
            return project.ProjectStatus == ProjectStatus.Done
                   || project.ProjectStatus == ProjectStatus.InProgress;
        }
    }
}