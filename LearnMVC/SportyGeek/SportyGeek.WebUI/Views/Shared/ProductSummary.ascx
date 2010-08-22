<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SportyGeek.Domain.Entities.Product>" %>
<div class="item">
    <h3><%: Model.Name%></h3>
    <%: Model.Description%>
    <h4><%: Model.Price.ToString("C")%></h4>
</div>