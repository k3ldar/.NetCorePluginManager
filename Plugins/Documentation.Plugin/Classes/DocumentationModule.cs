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
 *  Product:  Documentation Plugin
 *  
 *  File: DocumentationModule.cs
 *
 *  Purpose:  Class for documentation module
 *
 *  Date        Name                Reason
 *  18/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Provides a display class of documentation for a module.
    /// </summary>
    public sealed class DocumentationModule
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Title of module.</param>
        /// <param name="summary">Summary for module.</param>
        public DocumentationModule(in string title, in string summary)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
            Summary = summary ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Title of module
        /// </summary>
        /// <value>string</value>
        public string Title { get; private set; }

        /// <summary>
        /// Summary for module
        /// </summary>
        /// <value>string</value>
        public string Summary { get; private set; }

        #endregion Properties
    }
}
