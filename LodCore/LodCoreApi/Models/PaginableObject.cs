using LodCore.Infrastructure.DataAccess.Pagination;
using System.Collections.Generic;

namespace LodCoreApi.Models
{
    public class PaginableObject
    {
        public IEnumerable<IPaginable> Data;
        public int CountOfEntities;

        public PaginableObject(IEnumerable<IPaginable> data, int countOfEntities)
        {
            Data = data;
            CountOfEntities = countOfEntities;
        }
    }
}