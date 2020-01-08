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
 *  23/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;

namespace Blog.Plugin.Models
{
    /// <summary>
    /// View model for adding comments.
    /// </summary>
    public class CommentViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommentViewModel()
        {

        }

        /// <summary>
        /// Constructor for adding blog id to comment view model.
        /// </summary>
        /// <param name="blogId">Id of the blog the comment will be added to.</param>
        /// <param name="isLoggedIn">Indicates whether the current user is logged in or not.</param>
        public CommentViewModel(in int blogId, in bool isLoggedIn)
        {
            BlogId = blogId;
            IsLoggedIn = isLoggedIn;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Id of the blog the comment will be assigned to.
        /// </summary>
        /// <value>int</value>
        public int BlogId { get; set; }

        /// <summary>
        /// Comment to be added to the blog post.
        /// </summary>
        /// <value>string</value>
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// Determines whether the current user is logged in or not.
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        #endregion Properties
    }
}
