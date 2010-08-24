using System.Web.Mvc;
using SixBit.Web.CsJs;

namespace SportyGeek.WebUI.Views.Products
{
    class HelloWorld : BaseView
    {
        public override void Render(CsJsViewContext csjs)
        {
            csjs.Alert("Hello from csjs!");
        }
    }
}
