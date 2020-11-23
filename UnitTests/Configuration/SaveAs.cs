#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class ConfigurationTests
    {
        [TestMethod]
        public void Configuration_SaveAs() => ConfigSaveAs(false, false);

        [TestMethod]
        public void Configuration_SaveAs_UNC() => ConfigSaveAs(false, true);

        [TestMethod]
        public void Configuration_SaveAsWithLongPrefix() => ConfigSaveAs(true, false);

        [TestMethod]
        public void Configuration_SaveAsWithLongPrefix_UNC() => ConfigSaveAs(true, true);


        private static void ConfigSaveAs(in bool withPrefix, in bool asNetwork)
        {
            _ = Environment.OSVersion.Platform;

            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            IsNotNull(cfg);

            var (path, pathWithPrefix) = CreateLongTempFile(true, asNetwork: in asNetwork);
            Path.ChangeExtension(path, ".config");
            Path.ChangeExtension(pathWithPrefix, ".config");
            IsFalse(File.Exists(pathWithPrefix));

            cfg.SaveAs(withPrefix ? pathWithPrefix : path, ConfigurationSaveMode.Full, true);
            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}
#endif
