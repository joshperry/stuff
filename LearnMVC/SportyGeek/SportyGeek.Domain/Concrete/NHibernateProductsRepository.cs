using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using SportyGeek.Domain.Entities;

namespace SportyGeek.Domain.Concrete
{
    public class NHibernateProductsRepository : Abstract.IEntityRepository<Product>
    {
        ISession _session;

        public NHibernateProductsRepository(ISession session)
        {
            _session = session;
        }

        public IQueryable<Entities.Product> Query
        {
            get { return _session.Query<Product>(); }
        }

        public void Save(Product product)
        {
            return;
        }

        public void Delete(Product product)
        {
            _session.Delete(product);
        }
    }
}
