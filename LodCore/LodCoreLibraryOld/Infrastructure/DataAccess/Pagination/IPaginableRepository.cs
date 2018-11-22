using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Pagination
{
    public interface IPaginableRepository<T> where T : class 
    {
        int GetCountOfEntities(Expression<Func<T, bool>> criteria = null);
    }
}