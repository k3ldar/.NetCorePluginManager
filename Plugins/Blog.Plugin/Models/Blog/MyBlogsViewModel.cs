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
 *  File: CommentViewModel.cs
 *
 *  Purpose:  Blog comment view model
 *
 *  Date        Name                Reason
 *  24/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace Blog.Plugin.Models
{
    /// <summary>
    /// View model for viewing a specific users list of blogs.
    /// </summary>
    public class MyBlogsViewModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="modelData">BaseModelData items.</param>
        /// <param name="blogItems">User blog items.</param>
        public MyBlogsViewModel(BaseModelData modelData, List<BlogPostViewModel> blogItems)
            : base(modelData)
        {
            BlogItems = blogItems ?? throw new ArgumentNullException(nameof(blogItems));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// List of blog items for a specific user.
        /// </summary>
        /// <value>List&lt;BlogPostViewModel&gt;</value>
        public List<BlogPostViewModel> BlogItems { get; private set; }

        #endregion Properties
    }
}
