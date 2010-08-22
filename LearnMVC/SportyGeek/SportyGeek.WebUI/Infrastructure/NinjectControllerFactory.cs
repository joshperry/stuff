using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace SportyGeek.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        IKernel _kernel;
        public NinjectControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            object controller = null;
            if (controllerType == null || (controller = _kernel.Get(controllerType)) == null)
            {
                controller = base.GetControllerInstance(requestContext, controllerType);
            }
            return controller as IController;
        }

        protected override Type GetControllerType(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var controllerType = (from ct in _kernel.Get<IControllerFinder>().GetControllers()
                                  where ct.Name.StartsWith(controllerName)
                                  select ct).FirstOrDefault();

            if (controllerType == null)
                controllerType = base.GetControllerType(requestContext, controllerName);

            return controllerType;
        }
    }
}