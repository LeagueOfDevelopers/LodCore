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
    public class AllProjectsQuery
    {
        public IEnumerable<Project> Ask(string connectionString)
        {
            var allProjects = new List<Project>();
            var allProjectsDto = new List<ProjectDto>();

            using (var connection = new SqlConnection(connectionString))
            {
                allProjectsDto = connection.Query<ProjectDto>("SELECT * FROM projects").ToList();

                allProjectsDto.ForEach(p =>
                {
                    allProjects.Add((new Project(p.Name, 
                        new HashSet<ProjectType>(), 
                        p.Info, 
                        p.ProjectStatus,
                        new Common.Image(new Uri(p.BigPhotoUri), 
                        new Uri(p.SmallPhotoUri)),
                        new HashSet<Issue>(),
                        new HashSet<ProjectMembership>(), 
                        new HashSet<Common.Image>(), 
                        new HashSet<Common.ProjectLink>(),
                        new HashSet<Uri>()
                    )));
                });
            }

            return allProjects;
        }
    }
}
