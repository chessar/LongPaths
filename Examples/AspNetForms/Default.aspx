<%@ Page Language="C#" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>AspNetForms</title>
</head>
<body>
    <form id="Form1" runat="server">
        <span>CurrentDirectoryInfo.FullName = <% = HttpUtility.HtmlEncode(Utils.CurrentFolderFullName) %></span>
    </form>
</body>
</html>
