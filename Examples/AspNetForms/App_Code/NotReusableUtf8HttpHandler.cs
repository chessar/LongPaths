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