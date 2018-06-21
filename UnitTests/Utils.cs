using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using static Chessar.Hooks;
using static System.IO.Path;

namespace Chessar.UnitTests
{
    [TestClass]
    public static class Utils
    {
        #region Consts, Props

        internal const string
            LongPathPrefix = @"\\?\",
            UncLongPathPrefix = LongPathPrefix + @"UNC\",
            TenFileContent = "0123456789",
            XmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Root>
  <Element attr=""aValue"">Value</Element>
</Root>";
        internal static readonly UTF8Encoding
            Utf8WithoutBom = new UTF8Encoding(false);
        internal const AccessControlSections defaultAcs =
            AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group;
        internal static readonly XmlReaderSettings XmlSettings = new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Parse,
            XmlResolver = null
        };
        internal static string TempFolder { get; private set; }
        internal static string LongTempFolder { get; private set; }
        internal static string LongFolderName { get; private set; }
        internal static string WithPrefix(string path, in bool unc = false)
            => $"{(unc ? UncLongPathPrefix : LongPathPrefix)}{path}";
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static string GenerateRandomString() => $"{new Random(DateTime.UtcNow.Millisecond).Next():x}";
        internal static string RandomString => GenerateRandomString();
        internal static string RandomLongFolder => $"{LongTempFolder}{RandomString}";
        internal static string RandomLongTxtFile => $"{RandomLongFolder}.txt";

        #endregion

        [AssemblyInitialize]
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static void Init(TestContext testContext)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            TempFolder = Combine(GetTempPath(), RandomString).Trim('\\', '/', '?');
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            LongFolderName = new string('a', 254);
            var s = DirectorySeparatorChar;
            LongTempFolder = $"{TempFolder}{s}{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}{s}";

            PatchLongPaths();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            RemoveLongPathsPatch();

            if (Directory.Exists(TempFolder))
                Directory.Delete(WithPrefix(TempFolder), true);
        }

        internal static (string, string) CreateLongTempFolder(in bool skipCreate = false)
        {
            var path = RandomLongFolder;
            var pathWithPrefix = WithPrefix(path);
            if (!skipCreate)
                Directory.CreateDirectory(pathWithPrefix);
            return (path, pathWithPrefix);
        }

        internal static (string, string) CreateLongTempFile(in bool skipCreate = false)
        {
            var path = RandomLongTxtFile;
            var pathWithPrefix = WithPrefix(path);
            Directory.CreateDirectory(GetDirectoryName(pathWithPrefix));
            if (!skipCreate)
                File.CreateText(pathWithPrefix).Dispose();
            return (path, pathWithPrefix);
        }
    }
}
