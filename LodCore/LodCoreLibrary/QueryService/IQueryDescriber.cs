using LodCoreLibrary.QueryService.Queries;
using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService
{
    public interface IQueryDescriber
    {
        string Describe(AllProjectsQuery query);
        string Describe(GetProjectQuery query);
    }
}
