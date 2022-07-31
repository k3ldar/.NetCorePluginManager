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
 *  File: StockDataRow.cs
 *
 *  Purpose:  Table definition for product stock counts
 *
 *  Date        Name                Reason
 *  12/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameStock)]
    internal class StockDataRow : TableRowDefinition
    {
        private long _productId;
        private uint _quantity;

        [ForeignKey(Constants.TableNameProducts)]
        public long ProductId
        {
            get
            {
                return _productId;
            }

            set
            {
                if (_productId == value)
                    return;

                _productId = value;
                Update();
            }
        }
        public uint StockAvailability
        {
            get
            {
                return _quantity;
            }

            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
                Update();
            }
        }
    }
}
