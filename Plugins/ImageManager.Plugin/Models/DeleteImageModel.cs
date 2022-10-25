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
 *  Product:  Image Manager Plugin
 *  
 *  File: DeleteImageModel.cs
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace ImageManager.Plugin.Models
{
    /// <summary>
    /// Model used for deleting images
    /// </summary>
    public sealed class DeleteImageModel
    {
        /// <summary>
        /// Name of image
        /// </summary>
        /// <value>string</value>
        public string ImageName { get; set; }

        /// <summary>
        /// Name of group
        /// </summary>
        /// <value>string</value>
        public string GroupName { get; set; }

        /// <summary>
        /// Name of sub group
        /// </summary>
        /// <value>string</value>
        public string SubgroupName { get; set; }

        /// <summary>
        /// Check box confirming the item can be deleted
        /// </summary>
        /// <value>bool</value>
        public bool ConfirmDelete { get; set; }
    }
}
