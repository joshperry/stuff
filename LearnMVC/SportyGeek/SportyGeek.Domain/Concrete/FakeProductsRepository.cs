using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportyGeek.Domain.Abstract;
using SportyGeek.Domain.Entities;

namespace SportyGeek.Domain.Concrete
{
    public class FakeProductsRepository : IProductsRepository
    {
        static IQueryable<Product> fakeProducts = new List<Product>
        {
            new Product { Name = "Nerf Keyboard", Price = 25 },
            new Product { Name = "Tether Mouse", Price = 179 },
            new Product { Name = "DVD Frisbee", Price = 95 },
        }.AsQueryable();

        public IQueryable<Entities.Product> Products
        {
            get { return fakeProducts; }
        }
    }
}
