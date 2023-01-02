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
 *  Product:  Products.Plugin
 *  
 *  File: ProductSearchViewModel.cs
 *
 *  Purpose:  Base Product Model
 *
 *  Date        Name                Reason
 *  02/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
    /// <summary>
    /// Model used for advanced product searches
    /// </summary>
    public class ProductSearchViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductSearchViewModel()
        {
            ProductGroups = new List<CheckedViewItemModel>();
            Prices = new List<CheckedViewItemModel>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Text to be searched
        /// </summary>
        /// <value>string</value>
        [Display(Name = nameof(Languages.LanguageStrings.SearchDescription))]
        public string SearchText { get; set; }

        /// <summary>
        /// List of product groups that can be individually selected or not
        /// </summary>
        public List<CheckedViewItemModel> ProductGroups { get; set; }

        /// <summary>
        /// List of product price groupings that can be searched
        /// </summary>
        public List<CheckedViewItemModel> Prices { get; set; }

        /// <summary>
        /// Only show search results that contains video
        /// </summary>
        /// <value>bool</value>
        [Display(Name = nameof(Languages.LanguageStrings.SearchContainsVideo))]
        public bool ContainsVideo { get; set; }

        /// <summary>
        /// unique search name representing search options selected by the user
        /// </summary>
        public string SearchName { get; set; }

        /// <summary>
        /// Count of products with video content
        /// </summary>
        public int VideoProductCount { get; set; }

        #endregion Properties
    }
}
