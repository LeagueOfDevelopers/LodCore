using LodCoreLibraryOld.QueryService.Views.ProjectView;

namespace LodCoreLibraryOld.QueryService.Queries.ProjectQuery
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