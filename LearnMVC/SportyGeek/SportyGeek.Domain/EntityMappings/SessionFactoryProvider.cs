using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace SportyGeek.Domain.EntityMappings
{
    class SessionFactoryProvider
    {
        public static ISessionFactory GetSessionFactory()
        {
            // Use a local sqlite db file
            var dbconf = SQLiteConfiguration.Standard
                .ConnectionString(c => c.FromConnectionStringWithKey("ormConnection"));

            // Have fluent nhibernate automap our data model
            var domainAssembly = typeof(SportyGeek.Domain.Entities.Product).Assembly;
            var map = AutoMap
                .Assembly(domainAssembly) // look for the model in this assembly
                .Where(e => e.Namespace.EndsWith("Entities")) // where classes are in the Entities namespace
                .Conventions.AddAssembly(domainAssembly) // Detect any convention classes in the current assembly
                .UseOverridesFromAssemblyOf<SportyGeek.Domain.Entities.Product>(); // Detect and use override classes for some of the mapping attributes

            // Bring the DB and AutoMapping configuration together
            // to create an NHibernate configuration
            var factory = Fluently.Configure()
                .Database(dbconf)
                .Mappings(m => m.AutoMappings.Add(map))
                .ExposeConfiguration(BuildSchema) // Get access to the NHibernate config so that we can run SchemaExport
                .BuildSessionFactory(); // Return a SessionFactory that is completely configured

            return factory;
        }

        private static void BuildSchema(Configuration cfg)
        {
            // These methods are useful for rapid prototyping on your dev workstation.

            // This obliterates the schema and rebuilds it
            //new SchemaExport(cfg).Execute(false, true, false);

            // For less invasive changes this will try to
            // modify an existing schema keeping data intact
            new SchemaUpdate(cfg).Execute(false, true);

            // To create a DDL file to give to your DBA you can run SchemaExport like so:
            // new SchemaExport(cfg).SetOutputFile(@"C:\db.sql").Execute(false, false, false);
            // The DBA should be able to use a tool like SQLCompare to create a differencing script
            // that will merge the changes into a live DB
        }
    }
}
