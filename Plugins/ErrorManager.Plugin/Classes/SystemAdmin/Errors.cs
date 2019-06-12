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
 *  File: Errors.cs
 *
 *  Purpose:  Shows a list of errors in system admin console
 *
 *  Date        Name                Reason
 *  29/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Threading;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ErrorManager.Plugin.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of current errors that have been raised within ErrorManager.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public class Errors : SystemAdminSubMenu
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

        /// <summary>
        /// Returns error information raised within ErrorManager.Plugin.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            string Result = "Date|Error|Count";

            List<ErrorInformation> errors = ErrorManagerMiddleware.GetErrors();

            foreach (ErrorInformation item in errors)
            {
                Result += $"\r{item.Date.ToString(Thread.CurrentThread.CurrentUICulture.DateTimeFormat)}|" +
                    $"{item.Error.Message.Replace('|', ' ').Replace("\r\n", " ")}|{item.ErrorCount}";
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
            return ("Errors");
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

#pragma warning restore CS1591