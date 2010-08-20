using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportyGeek.Domain.Entities;

namespace SportyGeek.Domain.Abstract
{
    public interface IProductsRepository
    {
        IQueryable<Product> Products { get; }
    }
}
