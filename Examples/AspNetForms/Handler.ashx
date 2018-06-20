<%@ WebHandler Language="C#" Class="Handler" %>

using System.Web;
using static Utils;

public sealed class Handler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.Write(CurrentFolderFullName);
    }

    public bool IsReusable => false;
}