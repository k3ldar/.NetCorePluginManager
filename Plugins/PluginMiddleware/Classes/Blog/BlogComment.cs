/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Plugin Middleware
 *  
 *  File: BlogComment.cs
 *
 *  Purpose:  Middleware object for a blog entry
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Blog
{
	/// <summary>
	/// Comment made by a user for a blog entry
	/// </summary>
	public sealed class BlogComment
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public BlogComment()
		{

		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="id">Unique id of the comment.</param>
		/// <param name="parentComment">Parent comment id, if nested.</param>
		/// <param name="dateTime">Date and time comment made.</param>
		/// <param name="userId">Id of user making the comment.</param>
		/// <param name="userName">Name of person making the comment.</param>
		/// <param name="approved">Determines whether the comment is approved for display.</param>
		/// <param name="comment">Comment made by another user.</param>
		public BlogComment(in int id, in int? parentComment, in DateTime dateTime, in long userId,
			in string userName, in bool approved, in string comment)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentNullException(nameof(userName));

			if (String.IsNullOrEmpty(comment))
				throw new ArgumentNullException(nameof(comment));

			Comments = [];

			Id = id;
			ParentComment = parentComment;
			DateTime = dateTime;
			UserId = userId;
			Username = userName;
			Approved = approved;
			Comment = comment;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique id for the comment.
		/// </summary>
		/// <value>int</value>
		public int Id { get; set; }

		/// <summary>
		/// Id of the parent comment.  Allows for nesting comments.
		/// </summary>
		/// <value>int?</value>
		public int? ParentComment { get; set; }

		/// <summary>
		/// Date/Time the comment was made.
		/// </summary>
		/// <value>DateTime</value>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Name of user making the comment.
		/// </summary>
		/// <value>string</value>
		public string Username { get; set; }

		/// <summary>
		/// Unique user id of user making the comment.
		/// </summary>
		/// <value>long</value>
		public long UserId { get; set; }

		/// <summary>
		/// Provides an option for comments to be reviewed prior to being displayed.
		/// </summary>
		/// <value>bool</value>
		public bool Approved { get; set; }

		/// <summary>
		/// Comment made by a user.
		/// </summary>
		/// <value>string</value>
		public string Comment { get; set; }

		/// <summary>
		/// List of sub comments for the comment
		/// </summary>
		public List<BlogComment> Comments { get; set; }

		#endregion Properties
	}
}
