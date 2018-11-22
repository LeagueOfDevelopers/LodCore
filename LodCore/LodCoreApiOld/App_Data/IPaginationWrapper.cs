using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCoreApiOld.Models;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;

namespace LodCoreApiOld.App_Data
{
    public interface IPaginationWrapper<T> where T : class
    {
        PaginableObject WrapResponse(IEnumerable<IPaginable> content,Expression<Func<T, bool>> criteria = null);
    }
}