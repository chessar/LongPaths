using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
#if NET462
using static Chessar.Hooks;
#endif
using static System.IO.Path;

namespace Chessar.UnitTests
{
    [TestClass]
    public static class Utils
    {
        #region Consts, Props

        internal const string
            LongPathPrefix = @"\\?\",
            TenFileContent = "0123456789",
            XmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Root>
  <Element attr=""aValue"">Value</Element>
</Root>";
        internal static readonly char[]
            prefixChars = { DirectorySeparatorChar, AltDirectorySeparatorChar, '?', '.', ' ' },
            trimEndChars = { DirectorySeparatorChar, AltDirectorySeparatorChar, ' ' };
        internal static readonly UTF8Encoding
            Utf8WithoutBom = new UTF8Encoding(false);
#if NET462
        internal const AccessControlSections defaultAcs =
            AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group;
#endif
        internal static readonly XmlReaderSettings XmlSettings = new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Parse,
            XmlResolver = null
        };
        internal static readonly XmlWriterSettings XmlWSettings = new XmlWriterSettings
        {
            Encoding = Utf8WithoutBom,
            OmitXmlDeclaration = true
        };
        internal static string TempFolder { get; private set; }
        internal static string LongTempFolder { get; private set; }
        internal static string LongFolderName { get; private set; }
        internal static string RandomString => Guid.NewGuid().ToString();
        internal static string RandomLongFolder => $"{LongTempFolder}{RandomString}";
        internal static string RandomLongTxtFile => $"{RandomLongFolder}.txt";

        #endregion

        [AssemblyInitialize]
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static void Init(TestContext testContext)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            TempFolder = Combine(GetTempPath(), RandomString).Trim(prefixChars);
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            LongFolderName = new string('a', 254);
            var s = DirectorySeparatorChar;
            LongTempFolder = $"{TempFolder}{s}{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}{s}";

#if NET462
            PatchLongPaths();
#endif
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
#if NET462
            RemoveLongPathsPatch();
#endif

            if (Directory.Exists(TempFolder))
                Directory.Delete(TempFolder.WithPrefix(), true);
        }

        internal static string ToNetworkPath(this string path)
        {
            if (path is null)
                return null;
            path = path.TrimStart();
            if (path.Length < 2)
                return path;

            var s = DirectorySeparatorChar;

            if ((path[0] == s ||
                 path[0] == AltDirectorySeparatorChar) &&
                (path[1] == s ||
                 path[1] == AltDirectorySeparatorChar))
            {
                if (path.Length < 3)
                    return path;
                if (path[2] != '?')
                    return path;
                else if (path.Length > 7 && string.Equals("UNC", path.Substring(4, 3).ToUpperInvariant()))
                    path = path.Substring(8);
                else
                    path = path.TrimStart(prefixChars);
            }

            if (path[1] == VolumeSeparatorChar)
                path = $"localhost{s}{path[0]}${path.Substring(2)}";

            return $"{s}{s}{path}";
        }

        internal static string WithPrefix(this string path) => path
#if NET462
            .AddLongPathPrefix()
#endif
            ;

        internal static (string, string) CreateLongTempFolder(in bool skipCreate = false, in bool asNetwork = false, in bool withSlash = false)
        {
            var path = RandomLongFolder;
            var pathWithPrefix = path.WithPrefix();
            if (!skipCreate)
                Directory.CreateDirectory(pathWithPrefix);
            if (asNetwork)
                path = path.ToNetworkPath();
            path = path.TrimEnd(trimEndChars);
            pathWithPrefix = pathWithPrefix.TrimEnd(trimEndChars);
            if (withSlash)
            {
                path += DirectorySeparatorChar;
                pathWithPrefix += DirectorySeparatorChar;
            }
            return (path, asNetwork ? path
#if NET462
                .AddLongPathPrefix()
#endif
            : pathWithPrefix);
        }

        internal static (string, string) CreateLongTempFile(in bool skipCreate = false, in bool asNetwork = false)
        {
            var path = RandomLongTxtFile;
            var pathWithPrefix = path.WithPrefix();
            Directory.CreateDirectory(GetDirectoryName(pathWithPrefix));
            if (!skipCreate)
                File.CreateText(pathWithPrefix).Dispose();
            if (asNetwork)
                path = path.ToNetworkPath();
            return (path, asNetwork ? path
#if NET462
                    .AddLongPathPrefix()
#endif
                : pathWithPrefix);
        }
    }
}
