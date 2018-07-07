using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCoreApi.Models;
using LodCoreLibrary.Infrastructure.DataAccess.Pagination;

namespace LodCoreApi.App_Data
{
    public interface IPaginationWrapper<T> where T : class
    {
        PaginableObject WrapResponse(IEnumerable<IPaginable> content,Expression<Func<T, bool>> criteria = null);
    }
}