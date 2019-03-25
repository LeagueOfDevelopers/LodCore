using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCore.Infrastructure.DataAccess.Pagination;
using LodCoreApi.Models;

namespace LodCoreApi.Pagination
{
    public interface IPaginationWrapper<T> where T : class
    {
        PaginableObject WrapResponse(IEnumerable<IPaginable> content, Expression<Func<T, bool>> criteria = null);
    }
}