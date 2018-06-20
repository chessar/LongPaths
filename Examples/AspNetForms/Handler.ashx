<%@ WebHandler Language="C#" Class="Handler" %>

using System.Text;
using System.Web;

public sealed class Handler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.Write(Utils.CurrentFolderFullName);
    }

    public bool IsReusable { get { return false; } }
}