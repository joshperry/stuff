using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SixBit.Web.CsJs
{
    public abstract class BaseView : IView
    {
        public virtual void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            // We'll be writing out javascript
            viewContext.HttpContext.Response.ContentType = "text/javascript";

            // Wrap the code in a try/catch just to signal errors
            writer.WriteLine("try {");

            // Have the derived view render it's script into the context
            // then render it to the output stream
            var ctx = new CsJsViewContext(viewContext);
            Render(ctx);
            writer.Write(ctx.GetScript());

            // Close the script try/catch
            writer.Write("} catch(error) { alert('csjs error'); }");
        }

        public abstract void Render(CsJsViewContext csjs);
    }

    public abstract class BaseView<MT> : BaseView where MT : class
    {
        public override void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            Model = viewContext.Controller.ViewData.Model as MT;
            base.Render(viewContext, writer);
        }

        public MT Model { get; set; }
    }
}