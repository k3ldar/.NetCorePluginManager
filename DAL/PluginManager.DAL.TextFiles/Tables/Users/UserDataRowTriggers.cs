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
 *  File: UserDataRowTriggers.cs
 *
 *  Purpose:  Triggers for user table
 *
 *  Date        Name                Reason
 *  17/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	internal class UserDataRowTriggers : ITableTriggers<UserDataRow>
    {
        public int Position => 0;

        public TriggerType TriggerTypes => TriggerType.BeforeInsert;

        public void AfterDelete(List<UserDataRow> records)
        {
			// from interface but unused in this context
        }

		public void AfterInsert(List<UserDataRow> records)
        {

			// from interface but unused in this context
		}

		public void AfterUpdate(List<UserDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeDelete(List<UserDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeInsert(List<UserDataRow> records)
        {
            records.ForEach(r => r.PasswordExpire = DateTime.Now.AddYears(1));
        }

        public void BeforeUpdate(List<UserDataRow> records)
        {
			// from interface but unused in this context
		}

		public void BeforeUpdate(UserDataRow newRecord, UserDataRow oldRecord)
        {
			// from interface but unused in this context
		}
	}
}
