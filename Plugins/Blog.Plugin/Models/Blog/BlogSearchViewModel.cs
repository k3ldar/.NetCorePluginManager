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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Blog Plugin
 *  
 *  File: BlogSearchViewModel.cs
 *
 *  Purpose:  Blog search view model
 *
 *  Date        Name                Reason
 *  23/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace Blog.Plugin.Models
{
    /// <summary>
    /// Blog search view model
    /// </summary>
    public sealed class BlogSearchViewModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogSearchViewModel()
        {

        }

        /// <summary>
        /// Constructor for viewing search view.
        /// </summary>
        /// <param name="baseModelData"></param>
        public BlogSearchViewModel(BaseModelData baseModelData)
            : base(baseModelData)
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Tag to search for within all visible blogs.
        /// </summary>
        [Required]
        public string TagName { get; set; }

        #endregion Properties
    }
}
