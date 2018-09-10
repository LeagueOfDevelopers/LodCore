using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCore.Models;
using LodCoreLibrary.Infrastructure.DataAccess.Pagination;

namespace LodCore.Pagination
{
    public interface IPaginationWrapper<T> where T : class
    {
        PaginableObject WrapResponse(IEnumerable<IPaginable> content,Expression<Func<T, bool>> criteria = null);
    }
}