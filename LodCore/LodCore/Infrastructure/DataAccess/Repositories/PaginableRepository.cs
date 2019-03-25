using System;
using System.Linq.Expressions;
using LodCore.Infrastructure.DataAccess.Pagination;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class PaginableRepository<T> : IPaginableRepository<T> where T : class
    {
        public int GetCountOfEntities(Expression<Func<T, bool>> criteria = null)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();

            return criteria == null
                ? session.Query<T>().ToList().Count
                : session.Query<T>().Where(criteria).ToList().Count;*/
            return 0;
        }
    }
}