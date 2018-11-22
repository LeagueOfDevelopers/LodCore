using LodCore.Common;
using LodCore.Infrastructure.DataAccess.Pagination;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class PaginableRepository<T> : IPaginableRepository<T> where T : class
    {
        public PaginableRepository()
        {
        }

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