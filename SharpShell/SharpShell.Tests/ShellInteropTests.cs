using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using SharpShell.Interop;

namespace SharpShell.Tests
{
    [TestFixture]
    public class ShellInteropTests
    {
        [Test]
        public void CanGetKnownFolderPath()
        {
            //  We must be able to get the documents known path without throwing an exception.
            string path;
            Shell32.SHGetKnownFolderPath(KnownFolders.FOLDERID_Documents, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero, out path);
            ClassicAssert.That(path, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void CanGetAndFreeKnownFolderIdList()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Cookies, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero, out pidl);
            ClassicAssert.IsTrue(pidl != IntPtr.Zero);
            ClassicAssert.DoesNotThrow(() => Shell32.ILFree(pidl));
        }

        [Test]
        public void CanGetDesktopFolderLocationAndPath()
        {
            //  Asserts we can get the desktop folder pidl, get a path for it and free the pidl.
            IntPtr pidl;
            Shell32.SHGetFolderLocation(IntPtr.Zero, CSIDL.CSIDL_DESKTOP, IntPtr.Zero, 0, out pidl);
            var sb = new StringBuilder(260);
            ClassicAssert.IsTrue(Shell32.SHGetPathFromIDList(pidl, sb));
            ClassicAssert.That(sb.ToString(), Is.Not.Null.Or.Empty);
            ClassicAssert.DoesNotThrow(() => Shell32.ILFree(pidl));
        }

        [Test]
        public void CanEnumerateDesktopFolders()
        {
            //  Asserts that we can correctly use the IEnumIDList interface.

            //  Get the desktop folder.
            IShellFolder desktopFolder;
            Shell32.SHGetDesktopFolder(out desktopFolder);

            //  Create an enumerator and enumerate up to items.
            IEnumIDList enumerator;
            desktopFolder.EnumObjects(IntPtr.Zero, SHCONTF.SHCONTF_FOLDERS, out enumerator);
            uint fetched;
            var count = 20;
            IntPtr apidl = Marshal.AllocCoTaskMem(IntPtr.Size * count);
            enumerator.Next((uint)count, apidl, out fetched);
            var pidls = new IntPtr[fetched];
            Marshal.Copy(apidl, pidls, 0, (int)fetched);


            //  Assert the we can get the display name of each item.
            foreach (var pidl in pidls)
            {
                STRRET name;
                desktopFolder.GetDisplayNameOf(pidl, SHGDNF.SHGDN_NORMAL, out name);
                ClassicAssert.That(name.GetStringValue(), Is.Not.Null.Or.Empty);
                ClassicAssert.DoesNotThrow(() => Marshal.FreeCoTaskMem(pidl));
            }
            
        }
    }
}
