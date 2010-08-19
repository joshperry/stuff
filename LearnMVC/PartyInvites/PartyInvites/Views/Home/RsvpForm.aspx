<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PartyInvites.Models.GuestResponse>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>RsvpForm</title>
    <link type="text/css" rel="Stylesheet" href="~/Content/Site.css" />
</head>
<body>
    <h1>RSVP</h1>
    <% using (Html.BeginForm())
       { %>
       <%: Html.ValidationSummary() %>
       <p>Your name: <%: Html.TextBoxFor(x => x.Name) %></p>
       <p>Your email: <%: Html.TextBoxFor(x => x.Email) %></p>
       <p>Your phone: <%: Html.TextBoxFor(x => x.Phone) %></p>
       <p>
            Will you attend?
            <%: Html.DropDownListFor(x => x.WillAttend, new[] {
                new SelectListItem { Text="Yes, I'll be there", Value= bool.TrueString },
                new SelectListItem { Text="No, I can't come", Value= bool.FalseString },
             }, "Choose an option") %>       
       </p>
       <input type="submit" value="Submit RSVP" />
    <% } %>
</body>
</html>