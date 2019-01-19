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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminMainMenu.cs
 *
 *  Purpose:  System Admin Plugin Main Menu
 *
 *  Date        Name                Reason
 *  24/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace SharedPluginFeatures
{
    public sealed class SystemAdminMainMenu : BaseCoreClass, IComparable<SystemAdminMainMenu>
    {
        #region Constructors

        public SystemAdminMainMenu(in string name, in int uniqueId)
        {
            Name = name;
            UniqueId = uniqueId;
            ChildMenuItems = new List<SystemAdminSubMenu>();
        }

        #endregion Constructors

        #region Public Methods

        public string Area()
        {
            return (String.Empty);
        }

        public string Controller()
        {
            return ("SystemAdmin");
        }

        public string Action()
        {
            return ("Index");
        }

        public string BackColor()
        {
            return ("#707B7C");
        }

        public string ForeColor()
        {
            return ("white");
        }

        #endregion Public virtual Methods

        #region IComparable Methods

        public int CompareTo(SystemAdminMainMenu compareTo)
        {
            if (compareTo == null)
                return (1);

            int Result = SortOrder.CompareTo(compareTo.SortOrder);

            if (Result == 0)
                return (Name.CompareTo(compareTo.Name));

            return (Result);
        }

        #endregion IComparable

        #region Properties

        public List<SystemAdminSubMenu> ChildMenuItems { get; set; }

        public int UniqueId { get; set; }

        public string Name { get; private set; }

        public int SortOrder { get; set; }

        public Enums.SystemAdminMenuType MenuType { get; set; }

        public string Data { get; set; }

        public string Image { get; set; }

        #endregion Properties
    }
}
