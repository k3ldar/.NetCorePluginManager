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
 *  File: ResourceCategoryTriggers.cs
 *
 *  Purpose:  Triggers for resource categories
 *
 *  Date        Name                Reason
 *  01/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	internal class ResourceCategoryTriggers : ITableTriggers<ResourceCategoryDataRow>
	{
		public int Position => 0;

		public TriggerType TriggerTypes => TriggerType.BeforeInsert | TriggerType.BeforeUpdateCompare;

		public void AfterDelete(List<ResourceCategoryDataRow> records)
		{
			// from interface but unused in this context
		}

		public void AfterInsert(List<ResourceCategoryDataRow> records)
		{
			// from interface but unused in this context
		}

		public void AfterUpdate(List<ResourceCategoryDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeDelete(List<ResourceCategoryDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeInsert(List<ResourceCategoryDataRow> records)
		{
			foreach (ResourceCategoryDataRow row in records)
			{
				if (String.IsNullOrEmpty(row.RouteName))
					row.RouteName = SharedPluginFeatures.HtmlHelper.RouteFriendlyName(row.Name);
			}
		}

		public void BeforeUpdate(List<ResourceCategoryDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeUpdate(ResourceCategoryDataRow newRecord, ResourceCategoryDataRow oldRecord)
		{
			if (String.IsNullOrEmpty(newRecord.RouteName) && !String.IsNullOrEmpty(oldRecord.RouteName))
				newRecord.RouteName = oldRecord.RouteName;
			else if (!String.IsNullOrEmpty(newRecord.RouteName) && newRecord.RouteName != oldRecord.RouteName)
				newRecord.RouteName = oldRecord.RouteName;
		}
	}
}
