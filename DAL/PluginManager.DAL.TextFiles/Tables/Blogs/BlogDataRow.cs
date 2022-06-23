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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: BlogsDataRow.cs
 *
 *  Purpose:  Table definition for blogs
 *
 *  Date        Name                Reason
 *  06/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware.Blog;

using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameBlogs, CompressionType.Brotli)]
    internal sealed class BlogDataRow : TableRowDefinition
    {
        public BlogDataRow()
        {
            Comments = new List<BlogComment>();
        }

        /// <summary>
        /// Unique id of user creating the blog entry.
        /// </summary>
        [ForeignKey(Constants.TableNameUsers)]
        public long UserId { get; set; }

        /// <summary>
        /// Title of blog entry.
        /// </summary>
        /// <value>string</value>
        public string Title { get; set; }

        /// <summary>
        /// Brief description describing the blog entry.
        /// </summary>
        /// <value>string</value>
        public string Excerpt { get; set; }

        /// <summary>
        /// The main blog text.
        /// </summary>
        /// <value>string</value>
        public string BlogText { get; set; }

        /// <summary>
        /// Name of user creating the blog entry.
        /// </summary>
        /// <string>string</string>
        /// <value>string</value>
        public string Username { get; set; }

        /// <summary>
        /// Indicates whether the blog entry has been published or not.
        /// </summary>
        /// <value>bool</value>
        public bool Published { get; set; }

        /// <summary>
        /// The date/time the blog entry will appear live on the website.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime PublishDateTime { get; set; }

        /// <summary>
        /// Date and time the blog was created.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time the blog entry was last modified.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Descriptive tags for the blog.
        /// </summary>
        /// <value>List&lt;string&gt;</value>
        public List<string> Tags { get; set; }

        /// <summary>
        /// List of comments for the blog entry.
        /// </summary>
        /// <value>List&lt;BlogComment&gt;</value>
        public List<BlogComment> Comments { get; set; }
    }
}
