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
 *  Product:  SharedPluginFeatures
 *  
 *  File: DynamicContentPage.cs
 *
 *  Purpose:  Dynamic content page
 *
 *  Date        Name                Reason
 *  05/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;

using SharedPluginFeatures.DynamicContent;

namespace Middleware.DynamicContent
{
    /// <summary>
    /// Contains the contents for a dynamic page
    /// </summary>
    public sealed class DynamicContentPage : IDynamicContentPage
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicContentPage()
        {
            Content = new List<DynamicContentTemplate>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique page id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of dynamic page
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dynamic content that will be displayed within the page
        /// </summary>
        public List<DynamicContentTemplate> Content { get; private set; }

        #endregion Properties
    }
}
