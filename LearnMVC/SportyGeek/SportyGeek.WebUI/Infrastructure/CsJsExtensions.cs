using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportyGeek.WebUI.Infrastructure
{
    public static class CsJsExtensions
    {
        public static string RemoteAction(this UrlHelper url, string actionName)
        {
            return WrapUrl(url.Action(actionName, null, (RouteValueDictionary)null /* routeValues */));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, object routeValues)
        {
            return WrapUrl(url.Action(actionName, new RouteValueDictionary(routeValues)));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, RouteValueDictionary routeValues)
        {
            return WrapUrl(url.Action(actionName, routeValues));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, string controllerName)
        {
            return WrapUrl(url.Action(actionName, controllerName));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            return WrapUrl(url.Action(actionName, controllerName, new RouteValueDictionary(routeValues)));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return WrapUrl(url.Action(actionName, controllerName, routeValues));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, string controllerName, object routeValues, string protocol)
        {
            return WrapUrl(url.Action(actionName, controllerName, routeValues, protocol));
        }

        public static string RemoteAction(this UrlHelper url, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return WrapUrl(url.Action(actionName, controllerName, routeValues, protocol, hostName));
        }

        public static MvcHtmlString RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return MvcHtmlString.Create(RemoteActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary()));
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("Argument was null or empty", "linkText");
            }
            return GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null/* routeName */, actionName, controllerName, null /* protocol */, null /* hostName */, null /* fragment */, routeValues, htmlAttributes);
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return RemoteActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static string RemoteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("Argument was null or empty", "linkText");
            }
            return GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        private static string GenerateLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            
            string url = WrapUrl(UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, true));
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string WrapUrl(string url)
        {
            return string.Format("javascript:jQuery.getScript('{0}');", url);
        }
    }
}