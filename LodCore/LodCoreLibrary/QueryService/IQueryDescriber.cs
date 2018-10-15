using LodCoreLibrary.QueryService.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService
{
    public interface IQueryDescriber
    {
        string Describe(IQuery query);
    }
}
