using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using SportyGeek.Domain.Entities;

namespace SportyGeek.Domain.Concrete
{
    public class NHibernateProductsRepository : Abstract.IProductsRepository
    {
        public IQueryable<Entities.Product> Products
        {
            get { return EntityMappings.SessionFactoryProvider.GetSessionFactory().OpenSession().Query<Product>(); }
        }
    }
}
