using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SportyGeek.Domain.Entities;
using FluentNHibernate.Automapping.Alterations;

namespace SportyGeek.Domain.EntityMappings
{
    public class ProductMap : IAutoMappingOverride<Product>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<Product> automap)
        {
            automap.Map(p => p.Name)
                .Length(50);

            automap.Map(p => p.Description)
                .Length(500);

            automap.Map(p => p.Category)
                .Length(50);
        }
    }
}
