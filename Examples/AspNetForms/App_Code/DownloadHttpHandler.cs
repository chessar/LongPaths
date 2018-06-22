using System.Web;

public abstract class DownloadHttpHandler : NotReusableUtf8HttpHandler
{
    public sealed override void SendResponse(HttpContext context)
    {
        context.Response.AddHeader("Content-Disposition", "attachment; filename=file.txt");
        bool asNetwork = false, withPrefix = false;
        bool.TryParse(context.Request.Params["unc"], out asNetwork);
        bool.TryParse(context.Request.Params["withPrefix"], out withPrefix);
        SendFile(context, asNetwork, withPrefix);
    }

    public abstract void SendFile(HttpContext context, bool asNetwork, bool withPrefix);
}