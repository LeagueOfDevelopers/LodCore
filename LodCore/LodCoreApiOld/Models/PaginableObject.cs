using System.Collections.Generic;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;

namespace LodCoreApiOld.Models
{
    public class PaginableObject
    {
        public int CountOfEntities;
        public IEnumerable<IPaginable> Data;
    }
}