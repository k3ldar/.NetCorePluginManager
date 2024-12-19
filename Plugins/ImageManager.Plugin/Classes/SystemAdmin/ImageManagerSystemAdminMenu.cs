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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  ImageManager Plugin
 *  
 *  File: ImageManagerSystemAdminMenu.cs
 *
 *  Purpose:  Image manager system admin menu
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ImageManager.Plugin.Classes.SystemAdmin
{
	public sealed class ImageManagerSystemAdminMenu : SystemAdminSubMenu
	{
		public override string Action()
		{
			return nameof(Controllers.ImageManagerController.Index);
		}

		public override string Area()
		{
			return String.Empty;
		}

		public override string Controller()
		{
			return Controllers.ImageManagerController.Name;
		}

		public override string Data()
		{
			return String.Empty;
		}

		public override string Image()
		{
			return "/images/ImageManager/imMenu.png";
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.View;
		}

		public override string Name()
		{
			return Languages.LanguageStrings.AppImageManagement;
		}

		public override string ParentMenuName()
		{
			return Languages.LanguageStrings.AppImagesAll;
		}

		public override int SortOrder()
		{
			return 1000;
		}
	}
}

#pragma warning restore CS1591
