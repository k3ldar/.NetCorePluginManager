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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: SeoDataRow.cs
 *
 *  Purpose:  Table definition for seo data
 *
 *  Date        Name                Reason
 *  19/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameSeo, CompressionType.None, CachingStrategy.None)]
    internal class SeoDataRow : TableRowDefinition
    {
        public SeoDataRow()
        {
            Keywords = new List<string>();
        }

        public string Route { get; set; }

        public List<string> Keywords { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }
    }
}
