using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.Views;

namespace LodCoreLibrary.QueryService.Queries.ProjectQuery
{
    public class SaveProjectQuery : IQuery<ProjectSavedView>
    {
        public SaveProjectQuery(Project savingProject)
        {
            SavingProject = savingProject;

            SqlForGettingId = "INSERT INTO projects (name, info, projectstatus, bigphotouri, smallphotouri) " +
                    "VALUES(@Name, @Info, @ProjectStatus, @BigPhotoUri, @SmallPhotoUri); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

            SqlForScreenshots = "INSERT INTO screenshots (projectId, bigphotouri, smallphotouri) " +
                        $"VALUES(@ProjectId, @BigPhotoUri, @SmallPhotoUri);";
                        
            SqlForTypes = $"INSERT INTO projectTypes (projectId, type) VALUES(@ProjectId, @Type);";
        }

        public Project SavingProject { get; }
        public string SqlForGettingId { get; }
        public string SqlForScreenshots { get; }
        public string SqlForTypes { get; }
    }
}
