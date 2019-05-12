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
 *  Product:  SeoPlugin
 *  
 *  File: SeoCacheItem.cs
 *
 *  Purpose:  Internal class to hold cached Seo data
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace SeoPlugin
{
    internal sealed class SeoCacheItem
    {
        #region Constructors

        internal SeoCacheItem(in string title, in string metaDescription, in string author, in List<string> keywords)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            if (String.IsNullOrEmpty(metaDescription))
                throw new ArgumentNullException(nameof(metaDescription));

            if (keywords == null)
                throw new ArgumentNullException(nameof(keywords));

            Author = author ?? String.Empty;
            Title = title;
            Description = metaDescription;
            Keywords = String.Join(",", keywords);
        }

        #endregion Constructors
        
        #region Properties

        internal string Author { get; private set; }

        internal string Title { get; private set; }

        internal string Description { get; private set; }

        internal string Keywords { get; private set; }

        #endregion Properties
    }
}
