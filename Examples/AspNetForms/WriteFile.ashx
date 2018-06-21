<%@ WebHandler Language="C#" Class="WriteFile" %>

using System;
using System.Linq;
using System.Web;

public sealed class WriteFile : DownloadHttpHandler
{
    public override void SendFile(HttpContext context)
    {
        context.Response.WriteFile(Utils.CreateFileWithLongPath(),
            context.Request.Params.AllKeys.Contains("readIntoMemory", StringComparer.OrdinalIgnoreCase));
    }
}