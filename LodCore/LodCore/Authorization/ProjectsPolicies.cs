using LodCoreLibrary.Domain.ProjectManagment;

namespace LodCore.Authorization
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