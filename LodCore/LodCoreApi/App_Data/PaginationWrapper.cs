using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCoreApi.Models;
using LodCoreLibrary.Infrastructure.DataAccess.Pagination;

namespace LodCoreApi.App_Data
{
    public class PaginationWrapper<T> : IPaginationWrapper<T> where T : class
    {
        public PaginableObject WrapResponse(IEnumerable<IPaginable> content,Expression<Func<T, bool>> criteria = null)
        {
            var countOfEntities = _paginableRepository.GetCountOfEntities(criteria);

            return new PaginableObject()
            {
                Data = content,
                CountOfEntities = countOfEntities
            };
        }

        private readonly IPaginableRepository<T> _paginableRepository;

        public PaginationWrapper(IPaginableRepository<T> paginableRepository)
        {
            _paginableRepository = paginableRepository;
        }
    }
}