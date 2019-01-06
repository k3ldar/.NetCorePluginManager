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
    [Obsolete("This class is being removed in future version")]
    public abstract class SystemAdminMainMenu : BaseCoreClass, IComparable<SystemAdminMainMenu>
    {
        #region Public virtual Methods

        public virtual string Area()
        {
            return (String.Empty);
        }

        public virtual string Controller()
        {
            return ("SystemAdmin");
        }

        public virtual string Action()
        {
            return ("Index");
        }

        #endregion Public virtual Methods

        #region Public Abstract Methods

        public abstract string Name();

        public abstract int SortOrder();

        public abstract Enums.SystemAdminMenuType MenuType();

        public abstract string Data();

        public abstract string Image();

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

        public int CompareTo(SystemAdminMainMenu compareTo)
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

        public List<SystemAdminSubMenu> ChildMenuItems { get; set; }

        public int UniqueId { get; set; }

        #endregion Properties
    }
}
