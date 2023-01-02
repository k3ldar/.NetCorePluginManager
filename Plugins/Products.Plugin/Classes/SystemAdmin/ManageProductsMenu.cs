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
 *  Product:  Products.Plugin
 *  
 *  File: ManageProductsMenu.cs
 *
 *  Purpose:  System admin menu for managing products
 *
 *  Date        Name                Reason
 *  12/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

#pragma warning disable CS1591


namespace ProductPlugin.Classes.SystemAdmin
{
    public class ManageProductsMenu : SystemAdminSubMenu
	{
        public override string Controller()
        {
            return Controllers.ProductAdminController.Name;
        }

        public override string Action()
        {
            return nameof(Controllers.ProductAdminController.Index);
        }

		public override string Area()
		{
			return string.Empty;
		}

		public override string Name()
		{
			return Languages.LanguageStrings.Products;
		}

		public override int SortOrder()
		{
			return 10000;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.View;
		}

		public override string Image()
		{
			return "/Images/Products/prodAdmin.png";
		}

		public override string Data()
		{
			return string.Empty;
		}

		public override string ParentMenuName()
		{
			return Languages.LanguageStrings.ManageProducts;
		}
	}
}

#pragma warning restore CS1591
