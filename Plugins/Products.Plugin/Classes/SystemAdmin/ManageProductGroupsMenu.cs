﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Products.Plugin
 *  
 *  File: ManageProductGroupsMenu.cs
 *
 *  Purpose:  System admin menu for managing product groups
 *
 *  Date        Name                Reason
 *  12/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ProductPlugin.Classes.SystemAdmin
{
    public class ManageProductGroupsMenu : SystemAdminMainMenu
    {
        public ManageProductGroupsMenu()
            : base(Languages.LanguageStrings.AppProductGroups, -10000)
        {

        }

        public override string Controller()
        {
            return Controllers.ProductAdminController.Name;
        }

        public override string Action()
        {
            return nameof(Controllers.ProductAdminController.GroupIndex);
        }
    }
}

#pragma warning restore CS1591