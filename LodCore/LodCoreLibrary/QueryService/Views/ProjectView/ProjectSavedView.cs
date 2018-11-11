using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class ProjectSavedView
    {
        public ProjectSavedView(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
