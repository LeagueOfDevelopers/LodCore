using ProjectManagement.Domain;

namespace FrontendServices.App_Data.Authorization
{
    public static class ProjectsPolicies
    {
        public static bool OnlyPublic(Project project)
        {
            return project.AccessLevel == AccessLevel.Public;
        }

        public static bool OnlyDoneOrInProgress(Project project)
        {
            return project.ProjectStatus == ProjectStatus.Done
                   || project.ProjectStatus == ProjectStatus.InProgress;
        }
    }
}