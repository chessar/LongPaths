using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    [TestClass, TestCategory(nameof(ZipFile))]
    public sealed partial class ZipFileTests
    {
        private void ZipFileCreate(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            Path.ChangeExtension(path, ".zip");

            var (subPath, subPathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(subPathWithPrefix, TenFileContent);

            var (subFolder, subFolderWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            using (var zf = ZipFile.Create(withPrefix ? pathWithPrefix : path))
            {
                zf.BeginUpdate();
                zf.Add(withPrefix ? subPathWithPrefix : subPath);
                zf.AddDirectory(Path.GetDirectoryName(subPath));
                zf.CommitUpdate();
            }

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}
