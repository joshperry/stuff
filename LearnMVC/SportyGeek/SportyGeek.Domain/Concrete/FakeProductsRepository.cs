using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportyGeek.Domain.Abstract;
using SportyGeek.Domain.Entities;

namespace SportyGeek.Domain.Concrete
{
    public class FakeProductsRepository : IEntityRepository<Product>
    {
        static IQueryable<Product> fakeProducts = new List<Product>
        {
            new Product { Name = "Nerf Keyboard", Price = 25 },
            new Product { Name = "Tether Mouse", Price = 179 },
            new Product { Name = "DVD Frisbee", Price = 95 },
        }.AsQueryable();

        public IQueryable<Product> Query
        {
            get { return fakeProducts; }
        }

        public void Save(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
