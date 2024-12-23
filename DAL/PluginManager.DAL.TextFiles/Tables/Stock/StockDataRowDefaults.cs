﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: StockDataRowDefaults.cs
 *
 *  Purpose:  Default table definition for stock
 *
 *  Date        Name                Reason
 *  19/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	internal class StockDataRowDefaults : ITableDefaults<StockDataRow>
	{
		private readonly ISimpleDBOperations<ProductDataRow> _productDataRow;

		public StockDataRowDefaults(ISimpleDBOperations<ProductDataRow> productDataRow)
		{
			_productDataRow = productDataRow ?? throw new ArgumentNullException(nameof(productDataRow));
		}

		public long PrimarySequence => 1;

		public long SecondarySequence => 1;

		public ushort Version => 1;

		public List<StockDataRow> InitialData(ushort version)
		{
			if (version == 1)
			{
				List<StockDataRow> initialData = [];

				foreach (ProductDataRow item in _productDataRow.Select())
				{
					initialData.Add(new StockDataRow()
					{
						AutoRenew = false,
						MinimumStockLevel = 0,
						ReorderQuantity = 0,
						ProductId = item.Id,
						StoreId = 0
					});
				}

				return initialData;
			}

			return null;
		}
	}
}
