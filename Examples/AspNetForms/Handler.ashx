<%@ WebHandler Language="C#" Class="Handler" %>

using System.Web;

public sealed class Handler : NotReusableUtf8HttpHandler
{
    public override void SendResponse(HttpContext context)
    {
        context.Response.Write(Utils.CurrentFolderFullName);
    }
}