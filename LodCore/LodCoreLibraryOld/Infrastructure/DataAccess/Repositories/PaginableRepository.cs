using System;
using System.Linq;
using System.Linq.Expressions;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;
using NHibernate.Linq;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public class PaginableRepository<T> : IPaginableRepository<T> where T : class
    {
        private readonly IDatabaseSessionProvider _sessionProvider;

        public PaginableRepository(IDatabaseSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public int GetCountOfEntities(Expression<Func<T, bool>> criteria = null)
        {
            var session = _sessionProvider.GetCurrentSession();

            return criteria == null
                ? session.Query<T>().ToList().Count
                : session.Query<T>().Where(criteria).ToList().Count;
        }
    }
}