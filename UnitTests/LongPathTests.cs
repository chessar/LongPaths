using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using static Chessar.Hooks;
using static System.IO.Path;

namespace Chessar.UnitTests
{
    [TestClass]
    public sealed partial class LongPathTests
    {
        private const string
            prefix = @"\\?\",
            uncPrefix = @"\\?\UNC\",
            ten = "0123456789";

        private const AccessControlSections defaultAcs =
            AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group;

        private readonly string
            tempFolder = null,
            longTempFolder = null;

        private readonly UTF8Encoding enc;

        private static string WithPrefix(string path, in bool unc = false)
            => $"{(unc ? uncPrefix : prefix)}{path}";

        private static string RandomString => $"{new Random(DateTime.UtcNow.Millisecond).Next():x}";

        private string RandomLongFolder => $"{longTempFolder}{RandomString}";

        private string RandomLongTxtFile => $"{RandomLongFolder}.txt";

        public LongPathTests()
        {
            tempFolder = Combine(GetTempPath(), RandomString).Trim('\\', '/', '?');
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
            var part = new string('a', 254);
            var s = DirectorySeparatorChar;
            longTempFolder = $"{tempFolder}{s}{part}{s}{part}{s}{part}{s}{part}{s}";
            enc = new UTF8Encoding(false, false);
        }

        [TestInitialize]
        public void Init() => PatchLongPaths();

        [TestCleanup]
        public void Cleanup()
        {
            RemoveLongPathsPatch();

            if (Directory.Exists(tempFolder))
                Directory.Delete(WithPrefix(tempFolder), true);
        }

        private (string, string) CreateLongTempFolder(in bool skipCreate = false)
        {
            var path = RandomLongFolder;
            var pathWithPrefix = WithPrefix(path);
            if (!skipCreate)
                Directory.CreateDirectory(pathWithPrefix);
            return (path, pathWithPrefix);
        }

        private (string, string) CreateLongTempFile(in bool skipCreate = false)
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
