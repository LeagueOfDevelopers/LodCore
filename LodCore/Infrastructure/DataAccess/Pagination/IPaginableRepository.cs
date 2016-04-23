using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace DataAccess.Pagination
{
    public interface IPaginableRepository<T> where T : class 
    {
        int GetCountOfEntities(Expression<Func<T, bool>> criteria = null);
    }
}