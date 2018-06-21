using System.Web;

public abstract class DownloadHttpHandler : NotReusableUtf8HttpHandler
{
    public sealed override void SendResponse(HttpContext context)
    {
        context.Response.AddHeader("Content-Disposition", "attachment; filename=file.txt");
        SendFile(context);
    }

    public abstract void SendFile(HttpContext context);
}