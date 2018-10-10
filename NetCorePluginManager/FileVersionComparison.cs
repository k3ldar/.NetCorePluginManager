using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace AspNetCore.PluginManager
{
    internal sealed class FileVersionComparison : Comparer<FileInfo>
    {
        public override int Compare(FileInfo x, FileInfo y)
        {
            FileVersionInfo versionX = FileVersionInfo.GetVersionInfo(x.FullName);
            FileVersionInfo versionY = FileVersionInfo.GetVersionInfo(y.FullName);

            return (versionX.FileVersion.CompareTo(versionY.FileVersion));
        }
    }
}
