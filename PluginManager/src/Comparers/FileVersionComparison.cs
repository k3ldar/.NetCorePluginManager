﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: FileVersionComparison.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/09/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#pragma warning disable CS1591

namespace PluginManager
{
    public sealed class FileVersionComparison : Comparer<FileInfo>
    {
        public override int Compare(FileInfo objectCompare1, FileInfo objectCompare2)
        {
            if (objectCompare1 == null || objectCompare2 == null)
                return 0;

            FileVersionInfo versionX = FileVersionInfo.GetVersionInfo(objectCompare1.FullName);
            FileVersionInfo versionY = FileVersionInfo.GetVersionInfo(objectCompare2.FullName);

            return versionX.FileVersion.CompareTo(versionY.FileVersion);
        }

        public bool Equals(FileInfo objectCompare1, FileInfo objectCompare2)
        {
            if (objectCompare1 == null || objectCompare2 == null)
                return true;

            return Compare(objectCompare1, objectCompare2) == 0;
        }

        public bool Newer(FileInfo objectCompare1, FileInfo objectCompare2)
        {
            if (objectCompare1 == null || objectCompare2 == null)
                return true;

            return Compare(objectCompare1, objectCompare2) == 1;
        }
    }
}

#pragma warning restore CS1591