using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportyGeek.WebUI.Models;
using System.Text;
using System.Xml.Linq;
using System.Web.Routing;

namespace SportyGeek.WebUI.HtmlHelpers
{
    public static class PagingHelper 
    { 
        public static MvcHtmlString PageLinks(this HtmlHelper html,  
                                              PagingInfo pagingInfo, 
                                              Func<int, string> pageUrl, object attributes = null) 
        { 
            var xml = new XElement("ul",
                // Add any root attributes
                from kvp in new RouteValueDictionary(attributes)
                select new XAttribute(kvp.Key, kvp.Value),
                // Add an li for each page
                from i in Enumerable.Range(1, pagingInfo.TotalPages)
                select new XElement("li", 
                    i == pagingInfo.CurrentPage? new XAttribute("class", "selected"):null, // set the class attribute to "selected" for the current page
                    new XElement("a", new XAttribute("href", pageUrl(i)), i)
                )
            );
 
            return MvcHtmlString.Create(xml.ToString()); 
        } 
    }
}