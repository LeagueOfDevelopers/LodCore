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
    public class AllProjectsQuery : IQuery
    {
        string IQuery.GetType()
        {
            return GetType().ToString();
        }
    }
}
