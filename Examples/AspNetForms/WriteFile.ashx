<%@ WebHandler Language="C#" Class="WriteFile" %>

using System.Web;

public sealed class WriteFile : DownloadHttpHandler
{
    public override void SendFile(HttpContext context, bool asNetwork, bool withPrefix)
    {
        var readIntoMemory = false;
        bool.TryParse(context.Request.Params["readIntoMemory"], out readIntoMemory);
        context.Response.WriteFile(Utils.CreateFileWithLongPath(asNetwork, withPrefix), readIntoMemory);
    }
}