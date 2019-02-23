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
            Sql = "SELECT projects.ProjectId, Name, Info, ProjectStatus, projects.BigPhotoUri, projects.SmallPhotoUri, " +
                  "screenshots.BigPhotoUri, screenshots.SmallPhotoUri, projecttypes.id AS Type, projectmembership.DeveloperId, " +
                  "projectmembership.Role, Name, githubrepositorieslinks.id AS Uri FROM projects " +
                  "LEFT JOIN screenshots ON projects.ProjectId = screenshots.project_key " +
                  "LEFT JOIN projectmembership ON projects.ProjectId = projectmembership.ProjectId " +
                  "LEFT JOIN projecttypes ON projects.ProjectId = projecttypes.project_key " +
                  "LEFT JOIN githubrepositorieslinks ON githubrepositorieslinks.project_key = projects.ProjectId " +
                  "WHERE projects.ProjectId = @ProjectId";
        }

        public int ProjectId { get; }
        public string Sql { get; }
    }
}
