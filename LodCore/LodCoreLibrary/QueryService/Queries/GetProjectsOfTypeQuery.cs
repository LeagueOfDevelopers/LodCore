using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
{
    public class GetProjectsOfTypeQuery : IQuery<ProjectsOfTypeView>
    {
        public GetProjectsOfTypeQuery(ProjectType type)
        {
            Type = type;
            Sql = "SELECT * FROM projects AS Project " +
                "LEFT JOIN screenshots AS Screenshot ON Project.projectId = Screenshot.projectId " +
                "LEFT JOIN projectMemberships AS ProjMembership ON Project.projectId = ProjMembership.projectId " +
                "LEFT JOIN projectLinks AS Link ON Project.projectId = Link.projectId " +
                "LEFT JOIN projectTypes AS Type ON Project.projectId = Type.projectId " +
                $"WHERE Type.type = {Type};";
        }

        public ProjectType Type { get; }
        public string Sql { get; }
    }
}
