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
    public class GetProjectQuery
    {
        private int _projectId;

        public GetProjectQuery(int projectId)
        {
            _projectId = projectId;
        }

        public Project Ask(string connectionString)
        {
            Project project;
            ProjectDto projectDto;

            using (var connection = new SqlConnection(connectionString))
            {
               projectDto = connection.Query<ProjectDto>("SELECT * FROM Projects WHERE Id = @id", 
                    new { _projectId }).FirstOrDefault();
            }

            project = new Project(projectDto.Name, null, projectDto.Info, projectDto.ProjectStatus,
                new Common.Image(new Uri(projectDto.BigPhotoUri), new Uri(projectDto.SmallPhotoUri)), null, null, null, null, null);

            return project;
        }
    }
}
