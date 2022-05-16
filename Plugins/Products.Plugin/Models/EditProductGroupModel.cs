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
 *  Product:  Products.Plugin
 *  
 *  File: EditProductGroupModel.cs
 *
 *  Purpose:  Edit product group model
 *
 *  Date        Name                Reason
 *  09/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Middleware;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
    /// <summary>
    /// Model for editing/creating products
    /// </summary>
    public sealed class EditProductGroupModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Standard constructor
        /// </summary>
        public EditProductGroupModel()
        {

        }


        /// <summary>
        /// Constructor used for creating a product
        /// </summary>
        /// <param name="modelData"></param>
        public EditProductGroupModel(in BaseModelData modelData)
            : base(modelData)
        {
            Id = -1;
        }

        /// <summary>
        /// Constructor used for editing a product group
        /// </summary>
        /// <param name="modelData"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="showOnWebsite"></param>
        /// <param name="sortOrder"></param>
        /// <param name="tagLine"></param>
        /// <param name="url"></param>
        public EditProductGroupModel(in BaseModelData modelData, int id, string description,
            bool showOnWebsite, int sortOrder, string tagLine, string url)
            : base(modelData)
        {
            Id = id;
            Description = description;
            ShowOnWebsite = showOnWebsite;
            SortOrder = sortOrder;
            TagLine = tagLine;
            Url = url;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique product id.
        /// </summary>
        /// <value>int</value>
        public int Id { get; set; }

        /// <summary>
        /// Description of the product group.
        /// </summary>
        /// <value>string</value>
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(Languages.LanguageStrings.AppErrorInvalidProductGroupDescription))]
        [StringLength(30, MinimumLength = 8)]
        public string Description { get; set; }

        /// <summary>
        /// Determines whether the product group is visible on the website or not.
        /// </summary>
        /// <value>bool</value>
        public bool ShowOnWebsite { get; set; }

        /// <summary>
        /// Sort order in comparison to other product groups.
        /// </summary>
        /// <value>string</value>
        public int SortOrder { get; set; }

        /// <summary>
        /// Tag line displayed at the top of the page when thr group is shown.
        /// </summary>
        /// <value>string</value>
        [StringLength(250, MinimumLength = 0)]
        public string TagLine { get; set; }

        /// <summary>
        /// Custom url to be redirected to if the group is selected.  Default route values apply if not set.
        /// </summary>
        /// <value>string</value>
        [StringLength(250, MinimumLength = 0)]
        public string Url { get; set; }

        #endregion Properties
    }
}
