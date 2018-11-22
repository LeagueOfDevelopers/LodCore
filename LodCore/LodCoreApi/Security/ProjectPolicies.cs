using LodCore.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodCoreApi.Security
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
