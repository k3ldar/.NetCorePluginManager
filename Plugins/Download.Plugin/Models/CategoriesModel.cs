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
 *  Product:  Download Plugin
 *  
 *  File: CategoriesModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace DownloadPlugin.Models
{
    /// <summary>
    /// Download category view mode.
    /// </summary>
    public class CategoriesModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="name">Category name.</param>
        public CategoriesModel(in long id, in string name)
        {
            Id = id;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Category id.
        /// </summary>
        /// <value>int</value>
        public long Id { get; private set; }

        /// <summary>
        /// Category name.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        #endregion Properties
    }
}
