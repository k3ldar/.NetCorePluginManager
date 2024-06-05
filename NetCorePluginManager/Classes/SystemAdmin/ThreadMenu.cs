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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: ThreadMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of all threads and their current status that can be viewed within 
    /// SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public class ThreadMenu : SystemAdminSubMenu
    {
        public override string Action()
        {
            return String.Empty;
        }

        public override string Area()
        {
            return String.Empty;
        }

        public override string Controller()
        {
            return String.Empty;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Grid;
        }

        /// <summary>
        /// Returns delimited data on current active threads and their current status
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            StringBuilder Result = new("Name|Process Usage|System Usage|Thread Id|Cancelled|Unresponsive|Marked For Removal\r");

            Result.AppendFormat("Process||{0}%||||\r", ThreadManager.CpuUsage.ToString("F"));

            for (int i = 0; i < ThreadManager.ThreadCount; i++)
            {
                ThreadManager thread = ThreadManager.Get(i);

                string threadData = String.Format("{0}\r\n", thread.ToString());
                string[] parts = threadData.Split(';');

                Result.Append(SplitText(parts[1], ':') + "|");
                string cpu = SplitText(parts[0], ':');
				Result.Append(cpu.Substring(0, cpu.IndexOf("/")));
				Result.Append('|');
				Result.Append(cpu.Substring(cpu.IndexOf("/") + 1));
				Result.Append('|');
                Result.Append(SplitText(parts[2], ':') + "|");
                Result.Append(SplitText(parts[3], ':') + "|");
                Result.Append(SplitText(parts[4], ':') + "|");
                Result.Append(SplitText(parts[5], ':') + "\r");
            }

            return Result.ToString().Trim();
        }

        public override string Name()
        {
            return "Threads";
        }

        public override string ParentMenuName()
        {
            return "System";
        }

        public override int SortOrder()
        {
            return 0;
        }

        public override string Image()
        {
            return String.Empty;
        }

        private static string SplitText(string text, char splitText)
        {
            if (text.Contains(splitText.ToString()))
            {
                string result = text.Substring(text.IndexOf(splitText) + 1);
                return result.Trim();
            }
            else
                return text;
        }
    }
}

#pragma warning restore CS1591