/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Resources.Plugin
 *  
 *  File: EditResourceItemsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Resources.Plugin.Classes.SystemAdmin
{
	public class EditResourceItemsSubMenu : SystemAdminSubMenu
	{
		public override string Action()
		{
			return nameof(Controllers.ResourcesController.ManageResourceItems);
		}

		public override string Area()
		{
			return string.Empty;
		}

		public override string Controller()
		{
			return Controllers.ResourcesController.Name;
		}

		public override string Data()
		{
			return string.Empty;
		}

		public override string Image()
		{
			return string.Empty;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.View;
		}

		public override string Name()
		{
			return Languages.LanguageStrings.ManageResourceItems;
		}

		public override string ParentMenuName()
		{
			return Languages.LanguageStrings.ManageResources;
		}

		public override int SortOrder()
		{
			return 10000;
		}
	}
}

#pragma warning restore CS1591