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
 *  Product:  Documentation Plugin
 *  
 *  File: IndexViewModel.cs
 *
 *  Purpose:  View model for a index or document home
 *
 *  Date        Name                Reason
 *  18/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using DocumentationPlugin.Classes;

using SharedPluginFeatures;

namespace DocumentationPlugin.Models
{
    /// <summary>
    /// View model for displaying module summaries.
    /// </summary>
    public sealed class IndexViewModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelData">Base model data.</param>
        /// <param name="header">Header at the top of the page.</param>
        /// <param name="description">Description at the top of the page.</param>
        public IndexViewModel(in BaseModelData modelData,
            in string header, in string description)
            : base(modelData)
        {
            if (String.IsNullOrEmpty(header))
                throw new ArgumentNullException(nameof(header));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            AssemblyNames = new Dictionary<string, DocumentationModule>();
            DocumentationHeader = header;
            DocumentationDescription = description;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// List of all modules that have been loaded.
        /// </summary>
        /// <value>Dictionary&lt;string, DocumentationModule&gt;</value>
        public Dictionary<string, DocumentationModule> AssemblyNames { get; private set; }

        /// <summary>
        /// Header text that goes at the top of the page.
        /// </summary>
        /// <value>string</value>
        public string DocumentationHeader { get; private set; }

        /// <summary>
        /// Description which goes at the top of the page.
        /// </summary>
        /// <value>string</value>
        public string DocumentationDescription { get; private set; }

        /// <summary>
        /// Image to be displayed with the module.
        /// </summary>
        /// <value>string</value>
        public string Image { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Processes the image, if it doesn't exist returns a standard image.
        /// </summary>
        /// <param name="image">image to be displayed.</param>
        /// <returns>string</returns>
        public string ProcessImage(in string image)
        {
            if (String.IsNullOrEmpty(image))
                return "/images/docs/module.png";

            return image;
        }

        #endregion Methods        
    }
}
