﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCore.Models;
using LodCoreLibrary.Infrastructure.DataAccess.Pagination;

namespace LodCore.Pagination
{
    public class PaginationWrapper<T> : IPaginationWrapper<T> where T : class
    {
        public PaginableObject WrapResponse(IEnumerable<IPaginable> content,Expression<Func<T, bool>> criteria = null)
        {
            var countOfEntities = _paginableRepository.GetCountOfEntities(criteria);

            return new PaginableObject(content, countOfEntities);
        }

        private readonly IPaginableRepository<T> _paginableRepository;

        public PaginationWrapper(IPaginableRepository<T> paginableRepository)
        {
            _paginableRepository = paginableRepository;
        }
    }
}