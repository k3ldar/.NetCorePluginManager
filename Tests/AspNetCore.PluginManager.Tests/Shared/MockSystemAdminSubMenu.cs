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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockSystemAdminSubMenu.cs
 *
 *  Purpose:  Mock system admin sub menu class
 *
 *  Date        Name                Reason
 *  05/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockSystemAdminSubMenu : SystemAdminSubMenu
    {
        private readonly Enums.SystemAdminMenuType _menuType;
        private bool _overrideData = false;
        private string _data;

        public MockSystemAdminSubMenu(Enums.SystemAdminMenuType menuType)
        {
            _menuType = menuType;
        }

        public MockSystemAdminSubMenu(Enums.SystemAdminMenuType menuType,
            string data)
            : this(menuType)
        {
            _overrideData = true;
            _data = data;
        }

        public override string Action()
        {
            return "TestA";
        }

        public override string Area()
        {
            throw new NotImplementedException();
        }

        public override string Controller()
        {
            return "TestC";
        }

        public override string Data()
        {
            if (_overrideData)
                return _data;

            if (MenuType() == Enums.SystemAdminMenuType.Chart)
            {
                ChartModel chartModel = new ChartModel();
                chartModel.ChartTitle = "Test Title";
                chartModel.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.String, "Data1"));
                chartModel.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.Number, "Data 2"));
                chartModel.DataValues.Add("value 1", new List<decimal>() { 1.1m, 2.2m, 3.3m });
                chartModel.DataValues.Add("value 2", new List<decimal>() { 4.4m, 5.5m, 6.6m });

                return JsonConvert.SerializeObject(chartModel);
            }
            else if (MenuType() == Enums.SystemAdminMenuType.FormattedText)
            {
                return "Formatted Text";
            }
            else if (MenuType() == Enums.SystemAdminMenuType.Text)
            {
                return "Some data\tto\nwith <html>\rmarkup";
            }
            else if (MenuType() == Enums.SystemAdminMenuType.Map)
            {
                return "Map Data";
            }
            else if (MenuType() == Enums.SystemAdminMenuType.Grid)
            {
                return "Title1|Title2\rData1|Data2";
            }

            throw new NotImplementedException();
        }

        public override string Image()
        {
            throw new NotImplementedException();
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return _menuType;
        }

        public override string Name()
        {
            return "Mock Sub Menu";
        }

        public override string ParentMenuName()
        {
            throw new NotImplementedException();
        }

        public override int SortOrder()
        {
            throw new NotImplementedException();
        }
    }
}
