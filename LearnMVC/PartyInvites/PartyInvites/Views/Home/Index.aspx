<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body>
    <h1>Let's Party this <%: ViewData["Greeting"] %></h1>
    <p>
        We're going to have an exciting party!
    </p>
    <%: Html.ActionLink("RSVP Now", "RsvpForm") %>
</body>
</html>
