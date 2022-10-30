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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Search Plugin
 *  
 *  File: SearchCheckSubMenu.cs
 *
 *  Purpose:  Search Timings Sub Menu
 *
 *  Date        Name                Reason
 *  01/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SearchPlugin.Classes.Search;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SearchPlugin.Classes.SystemAdmin
{
    public class SearchTimings : SystemAdminSubMenu
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

        public override string Data()
        {
            string Result = "Setting|Value";

            Result += $"\rTotal Requests|{DefaultSearchProvider.SearchTimings.Requests}";
            Result += $"\rFastest ms|{DefaultSearchProvider.SearchTimings.Fastest}";
            Result += $"\rSlowest ms|{DefaultSearchProvider.SearchTimings.Slowest}";
            Result += $"\rAverage ms|{DefaultSearchProvider.SearchTimings.Average}";
            Result += $"\rTrimmed Avg ms|{DefaultSearchProvider.SearchTimings.TrimmedAverage}";
            Result += $"\rTotal ms|{DefaultSearchProvider.SearchTimings.Total}";

            return Result;
        }

        public override string Image()
        {
            return Constants.SystemImageStopWatch;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Grid;
        }

        public override string Name()
        {
            return nameof(Languages.LanguageStrings.Search);
        }

        public override string ParentMenuName()
        {
            return nameof(Languages.LanguageStrings.Timings);
        }

        public override int SortOrder()
        {
            return 0;
        }
    }
}

#pragma warning restore CS1591