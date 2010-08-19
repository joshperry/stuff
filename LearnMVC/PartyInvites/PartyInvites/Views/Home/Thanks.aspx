<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PartyInvites.Models.GuestResponse>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Thanks</title>
</head>
<body>
    <h1>Thank you, <%: Model.Name %></h1>
    <p>
    <% if (Model.WillAttend == true)
       { %>
       It's great that you're coming. The drinks are already in the fridge.
    <% }
       else
       { %>
       Sorry to hear that you can't make it, but thanks for letting us know.
    <% } %>
    </p>
</body>
</html>
