using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SportyGeek.Domain.Entities;

namespace SportyGeek.Domain.EntityMappings
{
    class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Map(x => x.Name)
                .Length(50);

            Map(x => x.Description)
                .Length(500);

            Map(x => x.Category)
                .Length(50);
        }
    }
}
