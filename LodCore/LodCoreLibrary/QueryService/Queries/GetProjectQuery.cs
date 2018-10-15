using Dapper;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
{
    public class GetProjectQuery : IQuery
    {
        public GetProjectQuery(int projectId)
        {
            ProjectId = projectId;
        }

        public int ProjectId { get; }

        string IQuery.GetType()
        {
            return GetType().ToString();
        }
    }
}
