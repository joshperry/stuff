using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBit.Web.CsJs
{
    public class CsJsViewContext
    {
        private System.Web.Mvc.ViewContext _viewContext;
        private StringBuilder _builder = new StringBuilder();

        public CsJsViewContext(System.Web.Mvc.ViewContext viewContext)
        {
            _viewContext = viewContext;
        }

        internal string GetScript()
        {
            return _builder.ToString();
        }

        public void Alert(string text)
        {
            JsFunc("alert", text);
        }

        private void JsFunc(string name, params string[] parameters)
        {
            _builder.Append("{0}(");
            if (parameters.Length > 0)
            {
                _builder.AppendFormat("'{0}'", string.Join("', '", parameters));
            }
            _builder.AppendLine(");");
        }
    }
}
