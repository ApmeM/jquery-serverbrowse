<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>jq-serverBrowser example</title>
    <script src="<%= ResolveUrl("~/")%>js/jquery.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/")%>js/jquery-ui.js" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/js/jquery.serverBrowser.js")%>" type="text/javascript"></script>
    <link href="~/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="~/css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $('#Button1').serverBrowser({
                onSelect: function(path) {
                    $('#TextBox1').val(path);
                },
                onLoad: function() {
                    return $('#TextBox1').val();
                },
                imageUrl: '<%=ResolveUrl("~/img/icons/") %>',
                systemImageUrl: '<%=ResolveUrl("~/img/browser/") %>',
                handlerUrl: '<%=ResolveUrl("~/GetPath.aspx") %>',
                title: 'Browse1',
                basePath: 'C:',
            });
        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="TextBox1" style="width: 500px" />
        <input id="Button1" type="button" class="ui-corner-all ui-state-default" value="Select" />
        <br />
    </div>
    </form>
</body>
</html>
