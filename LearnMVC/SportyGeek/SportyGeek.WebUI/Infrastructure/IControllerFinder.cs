using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportyGeek.WebUI.Infrastructure
{
    public interface IControllerFinder
    {
        IEnumerable<Type> GetControllers();
    }
}
