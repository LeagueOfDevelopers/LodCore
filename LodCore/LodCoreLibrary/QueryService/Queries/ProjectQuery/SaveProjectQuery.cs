using LodCoreLibrary.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries.ProjectQuery
{
    public class SaveProjectQuery
    {
        public SaveProjectQuery(Project savingProject)
        {
            SavingProject = savingProject;

            SqlForGettingId = "INSERT INTO projects (name, info, projectstatus, bigphotouri, smallphotouri) " +
                    "VALUES(@Name, @Info, @ProjectStatus, @BigPhotoUri, @SmallPhotoUri); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";
        }

        public Project SavingProject { get; }
        public string SqlForGettingId { get; }
    }
}
