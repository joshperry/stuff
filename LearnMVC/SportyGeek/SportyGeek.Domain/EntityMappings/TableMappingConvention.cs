using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;

namespace SportyGeek.Domain.EntityMappings
{
    // Convention for table mappings
    public class TableMappingConvention : IClassConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
        {
            // Make the table the plural version of the entity's name
            instance.Table(Inflector.Pluralize(instance.EntityType.Name));
        }
    }
}
