using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Pagination;
using FrontendServices.Models;

namespace FrontendServices.App_Data
{
    public interface IPaginationWrapper<T> where T : class
    {
        PaginableObject WrapResponse(IEnumerable<IPaginable> content,Expression<Func<T, bool>> criteria = null);
    }
}