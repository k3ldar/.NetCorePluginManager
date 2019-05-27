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
    /// <summary>
    /// container class for system wide menu items.
    /// 
    /// Although plugin modules can create as many instances of this type as they wish, it 
    /// is down the host application to determine how or if they are used.
    /// </summary>
    public sealed class SystemAdminMainMenu : BaseCoreClass, IComparable<SystemAdminMainMenu>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of menu item</param>
        /// <param name="uniqueId">Unique id given to the menu item to identify it.</param>
        public SystemAdminMainMenu(in string name, in int uniqueId)
        {
            Name = name;
            UniqueId = uniqueId;
            ChildMenuItems = new List<SystemAdminSubMenu>();
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Area to be used for controller when generating a Url if required.
        /// </summary>
        /// <returns></returns>
        public string Area()
        {
            return (String.Empty);
        }

        /// <summary>
        /// Controller to be called when the menu is clicked.
        /// </summary>
        /// <returns></returns>
        public string Controller()
        {
            return ("SystemAdmin");
        }

        /// <summary>
        /// Action to be called when the menu is clicked.
        /// </summary>
        /// <returns></returns>
        public string Action()
        {
            return ("Index");
        }

        /// <summary>
        /// Back color used when drawing the menu item.
        /// </summary>
        /// <returns></returns>
        public string BackColor()
        {
            return ("#707B7C");
        }

        /// <summary>
        /// Forecolor used when drawing the menu item.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Child menu items within the menu.
        /// </summary>
        /// <value>List&lt;SystemAdminSubMenu&gt;</value>
        public List<SystemAdminSubMenu> ChildMenuItems { get; set; }

        /// <summary>
        /// Unique id applied to the menu item.
        /// </summary>
        /// <value>int</value>
        public int UniqueId { get; set; }

        /// <summary>
        /// Name of the menu item.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        /// Sort order to be applied to the menu item.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Type of menu
        /// </summary>
        /// <value>SystemAdminMenuType</value>
        public Enums.SystemAdminMenuType MenuType { get; set; }

        /// <summary>
        /// Data to be returned by the menu item.
        /// </summary>
        /// <value>string</value>
        public string Data { get; set; }

        /// <summary>
        /// Image to be associated with the menu item.
        /// </summary>
        /// <value>string</value>
        public string Image { get; set; }

        #endregion Properties
    }
}
