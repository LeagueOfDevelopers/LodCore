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

namespace LodCoreLibrary.QueryService.Queries
{
    public class GetProjectQuery : IQuery<ProjectView>
    {
        public GetProjectQuery(int projectId)
        {
            ProjectId = projectId;
        }

        public int ProjectId { get; }
    }
}
