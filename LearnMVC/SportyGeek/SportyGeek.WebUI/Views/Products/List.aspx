<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SportyGeek.WebUI.Models.ProductListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Products
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% foreach (var product in Model.Products)
   { %>
   <% Html.RenderPartial("ProductSummary", product); %>
<% } %>
<%: Html.PageLinks(Model.PagingInfo, i => Url.Action("List", new {page = i}), new { Class = "pager" }) %>
</asp:Content>
