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
 *  File: InvoiceItemDataRowTriggers.cs
 *
 *  Purpose:  Triggers for invoice items table
 *
 *  Date        Name                Reason
 *  16/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class InvoiceItemDataRowTriggers : ITableTriggers<InvoiceItemDataRow>
    {
        public int Position => int.MinValue;

        public TriggerType TriggerTypes => TriggerType.BeforeDelete;

        public void AfterDelete(List<InvoiceItemDataRow> records)
        {
			// from interface but unused in this context
		}

		public void AfterInsert(List<InvoiceItemDataRow> records)
        {
			// from interface but unused in this context
		}

		public void AfterUpdate(List<InvoiceItemDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeDelete(List<InvoiceItemDataRow> records)
        {
            throw new InvalidDataRowException(nameof(InvoiceItemDataRow), nameof(InvoiceItemDataRow.Id), "Invoice items can not be deleted");
        }

        public void BeforeInsert(List<InvoiceItemDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeUpdate(List<InvoiceItemDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeUpdate(InvoiceItemDataRow newRecord, InvoiceItemDataRow oldRecord)
        {
			// from interface but unused in this context
		}
	}
}
