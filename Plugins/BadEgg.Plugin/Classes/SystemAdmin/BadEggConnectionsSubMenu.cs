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
 *  Product:  BadEgg.Plugin
 *  
 *  File: BadEggConnectionsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using SharedPluginFeatures;

namespace BadEgg.Plugin.Classes.SystemAdmin
{
    public sealed class BadEggConnectionsSubMenu : SystemAdminSubMenu
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
            StringBuilder Result = new StringBuilder("IP Address|Requests|Total Time|Created|Last Entry|Hits Per Minute|Results");

            string memStatus = WebDefender.ValidateConnections.GetMemoryStatus();

            foreach (string line in memStatus.Split('\r'))
            {
                Result.Append('\r');
                string[] entries = line.Split('#');

                for (int i = 0; i < entries.Length; i++)
                {
                    Result.Append(entries[i]);

                    if (i < (entries.Length -1))
                        Result.Append('|');
                }
            }
            //IpAddress: {0}; Requests: {1}; TotalTime: {2}; Created: {3}; LastEntry: {4}; " +
            //"HitsPerSecond: {5}; Results: {6}
            return (Result.ToString());
        }

        public override string Image()
        {
            return ("badegg");
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Name()
        {
            return ("Bad Egg Connections");
        }

        public override string ParentMenuName()
        {
            return ("System");
        }

        public override int SortOrder()
        {
            return (0);
        }
    }
}
