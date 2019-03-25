using System.Collections.Generic;
using LodCore.Infrastructure.DataAccess.Pagination;

namespace LodCoreApi.Models
{
    public class PaginableObject
    {
        public int CountOfEntities;
        public IEnumerable<IPaginable> Data;

        public PaginableObject(IEnumerable<IPaginable> data, int countOfEntities)
        {
            Data = data;
            CountOfEntities = countOfEntities;
        }
    }
}