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
 *  File: ProductDataRow.cs
 *
 *  Purpose:  Table definition for products
 *
 *  Date        Name                Reason
 *  26/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Tables.Products
{
    [Table(Constants.TableNameProducts, CompressionType.None, CachingStrategy.None)]
    internal class ProductDataRow : TableRowDefinition
    {
        [ForeignKey(Constants.TableNameProductGroups)]
        public int ProductGroupId { get; }

        public string Name { get; }

        public string Description { get; }

        public string Features { get; }

        public string VideoLink { get; }

        public bool NewProduct { get; }

        public bool BestSeller { get; }

        public decimal RetailPrice { get; }

        [UniqueIndex]
        public string Sku { get; }

        public bool IsDownload { get; }

        public bool AllowBackorder { get; }

        public uint StockAvailability { get; private set; }
    }
}
