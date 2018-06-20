using System;
using System.Web;
using static Utils;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) => Label1.Text =
        $"CurrentDirectoryInfo.FullName = {HttpUtility.HtmlEncode(CurrentFolderFullName)}";
}