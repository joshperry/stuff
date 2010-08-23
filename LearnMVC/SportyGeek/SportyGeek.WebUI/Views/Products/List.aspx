<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SportyGeek.WebUI.Models.ProductListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SportyGeek Chalet : <%: Model.CurrentCategory ?? "All Products" %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% foreach (var product in Model.Products)
   { %>
   <% Html.RenderPartial("ProductSummary", product); %>
<% } %>
<%: Html.PageLinks(Model.PagingInfo, i => Url.Action("List", new {page = i, category = Model.CurrentCategory}), new { Class = "pager" }) %>

<%: Html.RemoteActionLink("Hello", "HelloWorld") %>
</asp:Content>
