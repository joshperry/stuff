using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;

namespace SportyGeek.WebUI.Infrastructure
{
    public class ConventionControllerFinder : IControllerFinder
    {
        IEnumerable<Assembly> _assemblies;
        static IEnumerable<Type> _cache;
        public ConventionControllerFinder(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public IEnumerable<Type> GetControllers()
        {
            if (_cache == null)
                _cache = from a in _assemblies
                         from c in a.GetExportedTypes()
                         where c.IsPublic
                           && !c.IsAbstract
                           && !c.IsInterface
                           && c.Name.EndsWith("Controller")
                           && typeof(IController).IsAssignableFrom(c)
                         select c;

            return _cache;
        }
    }
}