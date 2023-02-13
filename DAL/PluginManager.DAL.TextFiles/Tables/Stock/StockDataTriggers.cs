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
 *  File: StockDataTriggers.cs
 *
 *  Purpose:  Triggers for stock table
 *
 *  Date        Name                Reason
 *  29/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables.Stock
{
	internal class StockDataTriggers : ITableTriggers<StockDataRow>
	{
		public int Position => Int32.MinValue;

		public TriggerType TriggerTypes => TriggerType.BeforeDelete;

		public void AfterDelete(List<StockDataRow> records)
		{
			// from interface but unused in this context
		}

		public void AfterInsert(List<StockDataRow> records)
		{
			// from interface but unused in this context
		}

		public void AfterUpdate(List<StockDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeDelete(List<StockDataRow> records)
		{
			if (records.Any(r => r.StockAvailability > 0))
				throw new InvalidDataRowException(nameof(StockDataRow), nameof(StockDataRow.StockAvailability), "Unable to delete a product when stock is available.");
		}

		public void BeforeInsert(List<StockDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeUpdate(List<StockDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeUpdate(StockDataRow newRecord, StockDataRow oldRecord)
		{
			// from interface but unused in this context
		}
	}
}
