using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.Results;
using DataAccess.Pagination;
using FrontendServices.Models;

namespace FrontendServices.App_Data
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