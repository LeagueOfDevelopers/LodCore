using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;
using System.Collections.Generic;

namespace LodCoreApiOld.Models
{
    public class PaginableObject
    {
        public IEnumerable<IPaginable> Data;

        public int CountOfEntities;
    }
}