/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Product:  SharedPluginFeatures
 *  
 *  File: Bootgrid.cs
 *
 *  Purpose:  Contains classes for use with boot grid
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

#pragma warning disable CS1591, IDE1006

namespace SharedPluginFeatures
{
    public class BootgridRequestData
    {
        public int current { get; set; }

        public int rowCount { get; set; }

        public string searchPhrase { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "ok on this occasion")]
        public string[] sort { get; set; }

        public IEnumerable<SortData> sortItems { get; set; }
    }

    public class BootgridResponseData<T> where T : class
    {
        public int current { get; set; }
        public int rowCount { get; set; }
        public IEnumerable<T> rows { get; set; }
        public int total { get; set; }
    }

    public class SortData
    {
        public string Field { get; set; }
        public string Type { get; set; }
    }

}

#pragma warning restore CS1591, IDE1006