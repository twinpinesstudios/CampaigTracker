using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Abstractions
{
    public interface IRepository<T>
        where T : class
    {
        public IQueryable<T> CreateQuery();
        public IEnumerable<T> Create(IEnumerable<T> collection);
        public T SingleAsync(Expression<Func<T, bool>> keyExpression);
        public T Single(object[] key);
        public bool UpdateAsync(IEnumerable<T> collection);
        public bool Delete(IEnumerable<T> collection);

    }
}
