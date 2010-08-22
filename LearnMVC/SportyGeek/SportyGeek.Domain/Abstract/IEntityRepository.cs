using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportyGeek.Domain.Abstract
{
    public interface IEntityRepository<T>
    {
        IQueryable<T> Query { get; }
        void Save(T entity);
        void Delete(T entity);
    }
}
