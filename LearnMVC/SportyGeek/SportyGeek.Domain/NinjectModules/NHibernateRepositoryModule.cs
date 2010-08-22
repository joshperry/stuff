using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using SportyGeek.Domain.Entities;
using SportyGeek.Domain.Abstract;
using SportyGeek.Domain.Concrete;
using SportyGeek.Domain.EntityMappings;

namespace SportyGeek.Domain.NinjectModules
{
    public class NHibernateRepositoryModule : NinjectModule
    {
        public override void Load()
        {
            // Provide the ISessionFactory for DI, someone will need to provide ISession with the proper scoping
            Kernel.Bind<NHibernate.ISessionFactory>()
                .ToMethod(c => SessionFactoryProvider.GetSessionFactory())
                .InSingletonScope();

            // Load all of the NHibernate based repositories into the kernel
            Kernel.Bind<IEntityRepository<Product>>().To<NHibernateProductsRepository>();
        }
    }
}
