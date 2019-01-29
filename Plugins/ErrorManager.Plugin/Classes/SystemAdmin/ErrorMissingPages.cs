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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  ErrorManager.Plugin
 *  
 *  File: ErrorMissingPages.cs
 *
 *  Purpose:  Shows a list of missing pages in system admin console
 *
 *  Date        Name                Reason
 *  29/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;


namespace ErrorManager.Plugin.Classes.SystemAdmin
{
    public class ErrorMissingPages : SystemAdminSubMenu
    {
        public override string Action()
        {
            return (String.Empty);
        }

        public override string Area()
        {
            return (String.Empty);
        }

        public override string Controller()
        {
            return (String.Empty);
        }

        public override string Data()
        {
            string Result = "Page|Count";

            Dictionary<string, uint> missingPages = ErrorManagerMiddleware.GetMissingPages();

            foreach (KeyValuePair<string, uint> item in missingPages)
            {
                Result += $"\r{item.Key}|{item.Value}";
            }

            return (Result);
        }

        public override string Image()
        {
            return (String.Empty);
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Name()
        {
            return ("Missing Links");
        }

        public override string ParentMenuName()
        {
            return ("Errors");
        }

        public override int SortOrder()
        {
            return (0);
        }
    }
}
