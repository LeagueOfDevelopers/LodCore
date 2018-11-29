using Dapper;
using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views;
using LodCore.QueryService.Views.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Queries.ProjectQuery
{
    public class GetProjectQuery : IQuery<FullProjectView>
    {
        public GetProjectQuery(int projectId)
        {
            ProjectId = projectId;
            Sql = "SELECT * FROM projects AS Project " +
                "LEFT JOIN screenshots AS Screenshot ON Project.projectId = Screenshot.project_key " +
                "LEFT JOIN projectMembership AS ProjMembership ON Project.projectId = ProjMembership.projectId " +
                "LEFT JOIN projecttypes AS Type ON Project.projectId = Type.project_key " +
                $"WHERE Project.projectId = {ProjectId};";
        }

        public int ProjectId { get; }
        public string Sql { get; }
    }
}
