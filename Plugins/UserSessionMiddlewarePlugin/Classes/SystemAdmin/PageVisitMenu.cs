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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: UserDetailsMenu.cs
 *
 *  Purpose:  Class for containing User session sales by country statistics
 *
 *  Date        Name                Reason
 *  29/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    public sealed class PageVisitMenu : SystemAdminSubMenu
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

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Data()
        {
            StringBuilder Result = new StringBuilder("Page|Total Visits");
            Dictionary<string, uint> pageVisits = new Dictionary<string, uint>();

            foreach (UserSession session in UserSessionManager.Clone)
            {
                foreach (PageViewData page in session.Pages)
                {
                    if (!pageVisits.ContainsKey(page.URL))
                        pageVisits.Add(page.URL, 0);

                    pageVisits[page.URL]++;
                }
            }

            foreach (KeyValuePair<string, uint> kvp in pageVisits)
            {
                Result.Append($"\r{kvp.Key}|{kvp.Value}");
            }

            return (Result.ToString().Trim());
        }

        public override string Name()
        {
            return ("Active Page Views");
        }

        public override string ParentMenuName()
        {
            return ("User Sessions");
        }

        public override int SortOrder()
        {
            return (0);
        }

        public override string Image()
        {
            return (String.Empty);
        }

        public override string BackColor()
        {
            if (ParentMenu != null)
                return (ParentMenu.BackColor());

            return ("#3498DB");
        }
    }
}
