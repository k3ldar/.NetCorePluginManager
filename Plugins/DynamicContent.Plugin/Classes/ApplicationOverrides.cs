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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: ApplicationOverrides.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  26/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using AppSettings;

using AspNetCore.PluginManager;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Classes
{
    public class ApplicationOverrides : IApplicationOverride
    {
        public bool ExpandApplicationVariable(string variableName, ref object value)
        {
            if (String.IsNullOrEmpty(variableName))
                throw new ArgumentNullException(nameof(variableName));

            if (variableName.Equals("RootPath"))
            {
                value = PluginManagerService.ApplicationRootPath;
                return true;
            }

            return false;
        }
    }
}

#pragma warning restore CS1591