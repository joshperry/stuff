<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SportyGeek.Domain.Entities.Product>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Products
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% foreach (var product in Model)
   { %>
    <div class="item">
        <h3><%: product.Name%></h3>
        <%: product.Description%>
        <h4><%: product.Price.ToString("C")%></h4>
    </div>
<% } %>
</asp:Content>
