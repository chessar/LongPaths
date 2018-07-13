// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Web;

public abstract class NotReusableUtf8HttpHandler : IHttpHandler
{
    public bool IsReusable { get { return false; } }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ClearContent();
        context.Response.SetPlainUtf8();
        SendResponse(context);
    }

    public abstract void SendResponse(HttpContext context);
}