using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using System.Reflection;
using System.Web.Mvc;

namespace SportyGeek.WebUI.Infrastructure
{
    public class NinjectControllerModule : NinjectModule
    {
        IControllerFinder _finder;
        public NinjectControllerModule(params Assembly[] assemblies)
        {
            _finder = new ConventionControllerFinder(assemblies);
        }

        public override void Load()
        {
            Kernel.Bind<IControllerFinder>().ToConstant(_finder);
            var controllers = _finder.GetControllers();

            foreach (var controller in controllers)
            {
                Kernel.Bind(controller).ToSelf();
            }
        }
    }
}