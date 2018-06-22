<%@ WebHandler Language="C#" Class="TransmitFile" %>

using System.Web;

public sealed class TransmitFile : DownloadHttpHandler
{
    public override void SendFile(HttpContext context, bool asNetwork, bool withPrefix)
    {
        context.Response.TransmitFile(Utils.CreateFileWithLongPath(asNetwork, withPrefix));
    }
}