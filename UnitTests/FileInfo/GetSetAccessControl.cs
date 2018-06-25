﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_GetSetAccessControl() => FileInfoGetSetAccessControl(false);

        [TestMethod]
        public void FileInfo_GetSetAccessControl_UNC() => FileInfoGetSetAccessControl(true);


        private void FileInfoGetSetAccessControl(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            var fs1 = fi.GetAccessControl();

            IsNotNull(fs1);

            var fs = new FileSecurity();
            fs.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            fi.SetAccessControl(fs);

            fi.Refresh();

            var fs2 = fi.GetAccessControl();

            IsNotNull(fs2);
        }
    }
}
