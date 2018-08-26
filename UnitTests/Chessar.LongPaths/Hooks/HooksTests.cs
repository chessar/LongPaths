#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using N = System.Func<string, bool, int, string>;
using R = System.Func<System.Web.HttpResponse, string, string>;
using S = System.Func<string, string>;

namespace Chessar.UnitTests.Hooks
{
    [TestClass, TestCategory(nameof(Hooks))]
    public sealed partial class HooksTests
    {
        private static readonly BindingFlags
            privateStatic = BindingFlags.NonPublic | BindingFlags.Static,
            privateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        private static N normalizePathPatched = null;
        private static S getFullPathInternalPatched = null;
        private static R getNormalizedFilenamePatched = null;
        private static MethodInfo needPatch = null;
        private static readonly string
            path = @"c:\path",
            relPath = "/path/relpath",
            url = "http://localhost/p a t h/";

        [ClassInitialize]
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static void Init(TestContext context)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var t = typeof(Chessar.Hooks);
            normalizePathPatched = (N)t
                .GetMethod("NormalizePathPatched", privateStatic)
                .CreateDelegate(typeof(N));
            getFullPathInternalPatched = (S)t
                .GetMethod("GetFullPathInternalPatched", privateStatic)
                .CreateDelegate(typeof(S));
            needPatch = t.GetMethod("NeedPatch", privateStatic);
            getNormalizedFilenamePatched = (R)t
                .GetMethod("GetNormalizedFilenamePatched", privateStatic)
                .CreateDelegate(typeof(R));

            InitHostingEnvironment();
        }

        private static void InitHostingEnvironment()
        {
            HostingEnvironment hostingEnvironment;
            if (!HostingEnvironment.IsHosted)
                new HostingEnvironment();
            var het = typeof(HostingEnvironment);
            var heField = het.GetField("_theHostingEnvironment", privateStatic);
            hostingEnvironment = (HostingEnvironment)heField.GetValue(null);
            var vpType = typeof(HttpContext).Assembly.GetType("System.Web.VirtualPath");
            var vp = vpType.GetConstructor(privateInstance, null, new[] { typeof(string) }, null).Invoke(new object[] { relPath });
            heField.SetValue(null, hostingEnvironment);
            var vpField = het.GetField("_appVirtualPath", privateInstance);
            vpField.SetValue(hostingEnvironment, vp);
            var ppField = het.GetField("_appPhysicalPath", privateInstance);
            var pp = Path.GetRandomFileName();
            pp = Path.Combine(Path.GetTempPath(), pp);
            if (!Directory.Exists(pp))
                Directory.CreateDirectory(pp);
            ppField.SetValue(hostingEnvironment, pp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static HttpContext GetContext() => new HttpContext(new FakeHttpWorkerRequest(relPath));

        private sealed class TestTraceException : Exception { }

        private static string GetTraceMessage(Action action)
        {
            var ttl = new TestTraceListener();
            Trace.Listeners.Add(ttl);
            try
            {
                action();
            }
            finally
            {
                Trace.Listeners.Remove(ttl);
            }
            var msg = ttl.LastMessage;
            return msg;
        }
    }
}
#endif