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
 *  File: VoucherDataRowTriggers.cs
 *
 *  Purpose:  Voucher triggers
 *
 *  Date        Name                Reason
 *  09/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal sealed class VoucherDataRowTriggers : ITableTriggers<VoucherDataRow>
    {
        public int Position => throw new NotImplementedException();

        public TriggerType TriggerTypes => TriggerType.BeforeUpdate | TriggerType.BeforeInsert;

        public void AfterDelete(List<VoucherDataRow> records)
        {
			// from interface but unused in this context
		}

		public void AfterInsert(List<VoucherDataRow> records)
        {
			// from interface but unused in this context
		}

		public void AfterUpdate(List<VoucherDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeDelete(List<VoucherDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeInsert(List<VoucherDataRow> records)
        {
            ValidateVoucherData(records);
        }

        public void BeforeUpdate(List<VoucherDataRow> records)
        {
            ValidateVoucherData(records);
        }

        public void BeforeUpdate(VoucherDataRow newRecord, VoucherDataRow oldRecord)
        {
			// from interface but unused in this context
		}

		private void ValidateVoucherData(List<VoucherDataRow> records)
        {
            foreach (VoucherDataRow record in records)
            {
                if (record.ValidFromTicks < DateTime.MinValue.Ticks)
                    record.ValidFromTicks = DateTime.MinValue.Ticks;

                if (record.ValidToTicks > DateTime.MaxValue.Ticks)
                    record.ValidToTicks = DateTime.MaxValue.Ticks;

                //individual product discount not yet supported
                record.ProductId = 0;
            }
        }

    }
}
