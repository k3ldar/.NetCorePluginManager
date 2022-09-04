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
 *  File: ResourceItemTriggers.cs
 *
 *  Purpose:  Triggers for resource items table
 *
 *  Date        Name                Reason
 *  04/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	internal class ResourceItemTriggers : ITableTriggers<ResourceItemDataRow>
	{
		public int Position => 0;

		public TriggerType TriggerTypes => TriggerType.BeforeInsert | TriggerType.BeforeUpdate;

		public void AfterDelete(List<ResourceItemDataRow> records)
		{

		}

		public void AfterInsert(List<ResourceItemDataRow> records)
		{

		}

		public void AfterUpdate(List<ResourceItemDataRow> records)
		{

		}

		public void BeforeDelete(List<ResourceItemDataRow> records)
		{

		}

		public void BeforeInsert(List<ResourceItemDataRow> records)
		{
			records.ForEach(r => ValidateUserName(r));
		}

		public void BeforeUpdate(List<ResourceItemDataRow> records)
		{
			records.ForEach(r => ValidateUserName(r));
		}

		public void BeforeUpdate(ResourceItemDataRow newRecord, ResourceItemDataRow oldRecord)
		{

		}

		private static void ValidateUserName(ResourceItemDataRow resourceItemDataRow)
		{
			if (String.IsNullOrEmpty(resourceItemDataRow.UserName))
				throw new InvalidDataRowException(nameof(resourceItemDataRow), nameof(resourceItemDataRow.UserName), "Can not be null or empty");
		}
	}
}
