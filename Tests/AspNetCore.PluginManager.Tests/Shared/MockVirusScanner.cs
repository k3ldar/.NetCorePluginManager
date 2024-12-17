/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockVirusScanner.cs
 *
 *  Purpose:  Mock IVirusScanner class
 *
 *  Date        Name                Reason
 *  02/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockVirusScanner : IVirusScanner
    {
        public MockVirusScanner()
        {
            ScannedItems = new List<string>();
        }

        public void ScanDirectory(in string directory)
        {
            ScannedItems.Add(directory);
        }

        public void ScanFile(in string fileName)
        {
            ScannedItems.Add(fileName);
        }

        public void ScanFile(in string[] fileNames)
        {
            foreach (string s in fileNames)
                ScannedItems.Add(s);
        }

        public List<string> ScannedItems { get; }
    }
}
