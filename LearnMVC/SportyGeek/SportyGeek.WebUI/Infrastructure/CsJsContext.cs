using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportyGeek.WebUI.Infrastructure
{
    public class CsJsContext
    {
        private System.Web.Mvc.ViewContext _viewContext;
        private StringBuilder _builder = new StringBuilder();

        public CsJsContext(System.Web.Mvc.ViewContext viewContext)
        {
            _viewContext = viewContext;
        }

        internal string GetScript()
        {
            return _builder.ToString();
        }

        public void Alert(string text)
        {
            _builder.AppendFormat("alert('{0}');", text);
            _builder.AppendLine();
        }
    }
}
