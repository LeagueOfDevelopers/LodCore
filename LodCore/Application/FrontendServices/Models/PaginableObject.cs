using System.Collections.Generic;
using DataAccess.Pagination;

namespace FrontendServices.Models
{
    public class PaginableObject
    {
        public IEnumerable<IPaginable> Data;

        public int CountOfEntities;
    }
}