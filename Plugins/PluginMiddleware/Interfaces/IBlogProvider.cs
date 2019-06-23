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
 *  Product:  Plugin Middleware
 *  
 *  File: IBlogProvider.cs
 *
 *  Purpose:  Net Core Plugin Manager Integration
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using Middleware.Blog;

namespace Middleware
{
    /// <summary>
    /// IBlogProvider interface provides methods used to manage blog posts.
    /// 
    /// This interface must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IBlogProvider
    {
        /// <summary>
        /// Returns the most recent blog entries
        /// </summary>
        /// <param name="recentCount">Number of recent entries to return.</param>
        /// <param name="publishedOnly">If true only returns published posts.</param>
        /// <returns>List&lt;BlogEntry&gt;</returns>
        List<BlogEntry> GetRecentPosts(in int recentCount, in bool publishedOnly);

        /// <summary>
        /// Retrieves an individual blog entry.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BlogEntry</returns>
        BlogEntry GetBlogEntry(in int id);

        /// <summary>
        /// Searches all blog items for blogs with a specific tag name.
        /// </summary>
        /// <param name="tagName">Name of tag to be searched for.</param>
        /// <returns>List&lt;BlogEntry&gt;</returns>
        List<BlogEntry> Search(in string tagName);

        /// <summary>
        /// Saves a blog entry.
        /// </summary>
        /// <param name="blogEntry">Blog entry to be saved.</param>
        void SaveBlogEntry(in BlogEntry blogEntry);
    }
}
