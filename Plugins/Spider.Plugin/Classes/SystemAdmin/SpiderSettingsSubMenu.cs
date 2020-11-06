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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Spider.Plugin
 *  
 *  File: SpiderSettingsSubMenu.cs
 *
 *  Purpose:  Displays robots editor
 *
 *  Date        Name                Reason
 *  14/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

using Spider.Plugin.Controllers;

#pragma warning disable CS1591

namespace Spider.Plugin.Classes.SystemAdmin
{
    public class SpiderSettingsSubMenu : SystemAdminSubMenu
    {
        public override String Action()
        {
            return nameof(SpiderController.Index);
        }

        public override String Area()
        {
            return String.Empty;
        }

        public override String Controller()
        {
            return SpiderController.Name;
        }

        public override String Data()
        {
            return String.Empty;
        }

        public override String Image()
        {
            return String.Empty;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.PartialView;
        }

        public override String Name()
        {
            return "Robots.txt";
        }

        public override String ParentMenuName()
        {
            return "System";
        }

        public override Int32 SortOrder()
        {
            return 0;
        }
    }
}

#pragma warning restore CS1591