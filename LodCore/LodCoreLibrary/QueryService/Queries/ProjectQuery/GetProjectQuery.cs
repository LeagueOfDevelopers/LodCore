using Dapper;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.DTOs;
using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries.ProjectQuery
{
    public class GetProjectQuery : IQuery<FullProjectView>
    {
        public GetProjectQuery(int projectId)
        {
            ProjectId = projectId;
            Sql = "SELECT * FROM projects AS Project " +
                "LEFT JOIN screenshots AS Screenshot ON Project.projectId = Screenshot.projectId " +
                "LEFT JOIN projectMemberships AS ProjMembership ON Project.projectId = ProjMembership.projectId " +
                "LEFT JOIN projectLinks AS Link ON Project.projectId = Link.projectId " +
                "LEFT JOIN projectTypes AS Type ON Project.projectId = Type.projectId " +
                $"WHERE Project.projectId = {ProjectId};";
        }

        public int ProjectId { get; }
        public string Sql { get; }
    }
}
