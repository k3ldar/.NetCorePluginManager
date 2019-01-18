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
 *  Product:  SharedPluginFeatures
 *  
 *  File: SystemAdminSubMenu.cs
 *
 *  Purpose:  System Admin Plugin Sub Main Menu
 *
 *  Date        Name                Reason
 *  24/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    public abstract class SystemAdminSubMenu : BaseCoreClass, IComparable<SystemAdminSubMenu>
    {
        #region Public Abstract Methods

        public abstract string Area();

        public abstract string Controller();

        public abstract string Action();

        public abstract string Name();

        public abstract int SortOrder();

        public abstract Enums.SystemAdminMenuType MenuType();

        public abstract string Image();

        public abstract string Data();

        public abstract string ParentMenuName();

        #endregion Public Abstract Methods

        #region Public Virtual Methods

        public virtual string BackColor()
        {
            return ("#707B7C");
        }

        public virtual string ForeColor()
        {
            return ("white");
        }

        #endregion Public Virtual Methods

        #region IComparable Methods

        public int CompareTo(SystemAdminSubMenu compareTo)
        {
            if (compareTo == null)
                return (1);

            int Result = SortOrder().CompareTo(compareTo.SortOrder());

            if (Result == 0)
                return (Name().CompareTo(compareTo.Name()));

            return (Result);
        }

        #endregion IComparable

        #region Properties

        public int UniqueId { get; set; }

        //public SystemAdminMainMenu ParentMenu { get; set; }

        #endregion Properties
    }
}
