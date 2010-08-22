using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportyGeek.WebUI.HtmlHelpers;
using System.Web.Mvc;
using SportyGeek.WebUI.Models;

namespace SportyGeek.Tests
{
    [TestClass]
    public class PagingLinkTests
    {
        [TestMethod]
        public void Can_generate_links_to_other_pages()
        {
            HtmlHelper html = null;

            var paging = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10,
            };
            Func<int, string> url = i => "Page" + i;

            MvcHtmlString result = html.PageLinks(paging, url);
            Assert.AreEqual(result.ToString(),
@"<ul>
  <li>
    <a href=""Page1"">1</a>
  </li>
  <li class=""selected"">
    <a href=""Page2"">2</a>
  </li>
  <li>
    <a href=""Page3"">3</a>
  </li>
</ul>");
        }

        [TestMethod]
        public void PagingInfo_calculates_total_pages_correctly()
        {
            var pagingInfo = new PagingInfo { TotalItems = 27, ItemsPerPage = 10 };
            Assert.AreEqual(pagingInfo.TotalPages, 3);
        }
    }
}
