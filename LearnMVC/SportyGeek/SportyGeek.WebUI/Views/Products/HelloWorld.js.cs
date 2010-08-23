using System.Web.Mvc;
using SportyGeek.WebUI.Infrastructure;

namespace SportyGeek.WebUI.Views.Products
{
    class HelloWorld : CsJsView
    {
        public override void Render(CsJsContext csjs)
        {
            csjs.Alert("Hello from csjs!");
            
        }
    }
}
