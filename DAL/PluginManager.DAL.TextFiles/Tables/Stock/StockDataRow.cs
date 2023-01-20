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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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
		private uint _minimumStockLevel;
		private uint _reorderQuantity;
		private long _storeId;
		private bool _autoRenew;

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

		[ForeignKey(Constants.TableNameStore)]
		public long StoreId
		{
			get => _storeId;

			set
			{
				if (value == _storeId)
					return;

				_storeId = value;
				Update();
			}
		}

		public uint	MinimumStockLevel
		{
			get => _minimumStockLevel;

			set
			{
				if (value == _minimumStockLevel)
					return;

				_minimumStockLevel = value;
				Update();
			}
		}

		public uint ReorderQuantity
		{
			get => _reorderQuantity;

			set
			{
				if (value == _reorderQuantity)
					return;

				_reorderQuantity = value;
				Update();
			}
		}

		public bool AutoRenew
		{
			get => _autoRenew;

			set
			{
				if (value == _autoRenew)
					return;

				_autoRenew = value;
				Update();
			}
		}
	}
}
