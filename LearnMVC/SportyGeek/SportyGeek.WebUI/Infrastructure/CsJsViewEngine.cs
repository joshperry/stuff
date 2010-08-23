using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportyGeek.WebUI.Infrastructure
{
    public class CsJsViewEngine : VirtualPathProviderViewEngine
    {
        public CsJsViewEngine()
        {
            this.ViewLocationFormats = new string[]{"~/Views/{1}/{0}.js.cs", "~/Views/Shared/{0}.js.cs"};
            this.PartialViewLocationFormats = new string[] { "~/Views/{1}/_{0}.js.cs", "~/Views/Shared/_{0}.js.cs" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            throw new NotImplementedException();
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            throw new NotImplementedException();
        }
    }
}