using LodCoreLibrary.Infrastructure.DataAccess.Pagination;
using System.Collections.Generic;

namespace LodCore.Models
{
    public class PaginableObject
    {
        public IEnumerable<IPaginable> Data;

        public int CountOfEntities;
    }
}